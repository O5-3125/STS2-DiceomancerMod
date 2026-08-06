using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Events;

[RegisterActEvent(typeof(Glory))]
public sealed class Copier : ModEventTemplate
{
    // 背景图位置,目前没有背景图所以暂时用TestEvent.png代替
    public override EventAssetProfile AssetProfile => new(
        InitialPortraitPath: "res://Diceomancer/images/Event/TestEvent.png"
    );

    // 设置一些数值
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new GoldVar(50),
        new GoldVar("KickGold", 60),
        new HpLossVar(6m)
    ];

    // 至少要付得起复印费
    public override bool IsAllowed(IRunState runState)
    {
        return runState.Players.All(p => p.Gold >= DynamicVars.Gold.BaseValue);
    }

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            new EventOption(this, Copy, InitialOptionKey("COPY")),
            new EventOption(this, Kick, InitialOptionKey("KICK"))
        ];
    }

    // 复印一个！失去50金币，选择1张牌复制加入牌组
    private async Task Copy()
    {
        await PlayerCmd.LoseGold(DynamicVars.Gold.BaseValue, Owner!, GoldLossType.Stolen);

        var selected = await CardSelectCmd.FromDeckGeneric(Owner!,
            new CardSelectorPrefs(new LocString(LocTable, $"{Id.Entry}.pages.INITIAL.options.COPY.prompt"), 1),
            c => c.Type != CardType.Quest);
        var cardToCopy = selected.FirstOrDefault();
        if (cardToCopy != null)
        {
            var copy = Owner!.RunState.CloneCard(cardToCopy);
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(copy, PileType.Deck), 2f);
        }

        SetEventFinished(L10NLookup($"{Id.Entry}.pages.COPY_DONE.description"));
    }

    // 回旋踢一脚看看能不能掉点钱，50%成功获得60金币，失败失去6生命
    private async Task Kick()
    {
        if (Owner.RunState.Rng.Niche.NextBool())
        {
            await PlayerCmd.GainGold(DynamicVars["KickGold"].BaseValue, Owner!);
            SetEventFinished(L10NLookup($"{Id.Entry}.pages.KICK_SUCCESS.description"));
        }
        else
        {
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner!.Creature,
                DynamicVars.HpLoss.IntValue, ValueProp.Unblockable | ValueProp.Unpowered, null, null, null);
            SetEventFinished(L10NLookup($"{Id.Entry}.pages.KICK_FAILURE.description"));
        }
    }
}
