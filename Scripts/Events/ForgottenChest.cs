using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Events;

[RegisterActEvent(typeof(Glory))]
[RegisterActEvent(typeof(Overgrowth))]
public sealed class ForgottenChest : ModEventTemplate
{
    // 背景图位置,目前没有背景图所以暂时用TestEvent.png代替
    public override EventAssetProfile AssetProfile => new(
        InitialPortraitPath: "res://Diceomancer/images/Event/TestEvent.png"
    );

    // 设置一些数值
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new GoldVar(50),
        new DamageVar(5m, ValueProp.Unblockable | ValueProp.Unpowered)
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            new EventOption(this, Pry, InitialOptionKey("PRY")),
            new EventOption(this, Smash, InitialOptionKey("SMASH"))
        ];
    }

    // 我就撬！50%概率成功获得随机遗物，失败失去5生命
    private async Task Pry()
    {
        if (Owner.RunState.Rng.Niche.NextBool())
        // if (true)
        {
            var relic = RelicFactory.PullNextRelicFromFront(base.Owner).ToMutable();
            await RelicCmd.Obtain(relic, Owner!);
            SetEventFinished(L10NLookup($"{Id.Entry}.pages.SUCCESS.description"));
        }
        else
        {
            await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner!.Creature, DynamicVars.Damage, null, null);
            SetEventFinished(L10NLookup($"{Id.Entry}.pages.FAIL.description"));
        }
    }

    // 赶在安全锁触发前直接砸碎宝箱！获得50金币
    private async Task Smash()
    {
        await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner!);
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.SMASH_DONE.description"));
    }
}
