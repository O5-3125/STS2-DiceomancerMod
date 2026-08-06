using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Acts;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Events;

[RegisterActEvent(typeof(Hive))]
public sealed class FlyRestaurant : ModEventTemplate
{
    // 背景图位置,目前没有背景图所以暂时用TestEvent.png代替
    public override EventAssetProfile AssetProfile => new(
        InitialPortraitPath: "res://Diceomancer/images/Event/TestEvent.png"
    );

    // 设置一些数值
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HealVar(10),
        new HealVar("FeastHeal", 50),
        new MaxHpVar("RiceMaxHp", 20)
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            new EventOption(this, Wrap, InitialOptionKey("WRAP")),
            new EventOption(this, Feast, InitialOptionKey("FEAST")),
            new EventOption(this, Rice, InitialOptionKey("RICE"))
        ];
    }

    // 点个卷饼吃！获得10生命
    private async Task Wrap()
    {
        await CreatureCmd.Heal(Owner!.Creature, DynamicVars.Heal.BaseValue);
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.WRAP_DONE.description"));
    }

    // 点顿大餐吃！80%概率获得50生命
    private async Task Feast()
    {
        if (Owner.RunState.Rng.Niche.NextInt(100) < 80)
        {
            await CreatureCmd.Heal(Owner!.Creature, DynamicVars["FeastHeal"].BaseValue);
            SetEventFinished(L10NLookup($"{Id.Entry}.pages.SUCCESS.description"));
        }
        else
        {
            SetEventFinished(L10NLookup($"{Id.Entry}.pages.FAIL.description"));
        }
    }

    // 点个盖饭吃！30%概率获得20最大生命值
    private async Task Rice()
    {
        if (Owner.RunState.Rng.Niche.NextInt(100) < 30)
        {
            await CreatureCmd.GainMaxHp(Owner!.Creature, DynamicVars["RiceMaxHp"].BaseValue);
            SetEventFinished(L10NLookup($"{Id.Entry}.pages.SUCCESS.description"));
        }
        else
        {
            SetEventFinished(L10NLookup($"{Id.Entry}.pages.FAIL.description"));
        }
    }
}