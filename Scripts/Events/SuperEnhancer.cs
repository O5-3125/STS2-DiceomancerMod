using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Events;

[RegisterActEvent(typeof(Glory))]
public sealed class SuperEnhancer : ModEventTemplate
{
    // 背景图位置,目前没有背景图所以暂时用TestEvent.png代替
    public override EventAssetProfile AssetProfile => new(
        InitialPortraitPath: "res://Diceomancer/images/Event/TestEvent.png"
    );

    // 设置一些数值
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new GoldVar(30),
        new HpLossVar(4m)
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        // 钱不够时锁定付出金钱选项
        var payGold = Owner != null && Owner.Gold < DynamicVars.Gold.BaseValue
            ? new EventOption(this, null, InitialOptionKey("PAY_GOLD"))
            : new EventOption(this, PayGold, InitialOptionKey("PAY_GOLD"));
        return
        [
            payGold,
            new EventOption(this, PayHp, InitialOptionKey("PAY_HP")),
            new EventOption(this, Leave, InitialOptionKey("LEAVE"))
        ];
    }

    // 付出金钱！失去30金币，50%概率升级随机卡牌，然后重复本场景
    private async Task PayGold()
    {
        await PlayerCmd.LoseGold(DynamicVars.Gold.BaseValue, Owner!, GoldLossType.Stolen);
        if (Owner.RunState.Rng.Niche.NextBool())
        {
            await UpgradeRandomCard();
        }

        SetEventState(InitialDescription, GenerateInitialOptions());
    }

    // 付出生命！失去4生命，50%概率升级随机卡牌，然后重复本场景
    private async Task PayHp()
    {
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner!.Creature, DynamicVars.HpLoss.IntValue,
            ValueProp.Unblockable | ValueProp.Unpowered, null, null, null);
        if (Owner.RunState.Rng.Niche.NextBool())
        {
            await UpgradeRandomCard();
        }

        SetEventState(InitialDescription, GenerateInitialOptions());
    }

    // 走了！离开
    private Task Leave()
    {
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.LEAVE.description"));
        return Task.CompletedTask;
    }

    // 升级一张随机的可升级卡牌
    private async Task UpgradeRandomCard()
    {
        var upgradable = PileType.Deck.GetPile(Owner!).Cards.Where(c => c.IsUpgradable).ToList();
        if (upgradable.Count == 0)
        {
            return;
        }

        var card = upgradable.StableShuffle(Owner!.RunState.Rng.Niche).First();
        CardCmd.Upgrade(card);
        await Task.CompletedTask;
    }
}