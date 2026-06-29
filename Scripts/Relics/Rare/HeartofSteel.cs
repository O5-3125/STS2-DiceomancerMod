using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Rare;

[RegisterRelic(typeof(DiceomancerRelicPool))]
public class HeartOfSteel : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    // 小图标（原版85x85）
    // public override string PackedIconPath => $"res://Diceomancer/images/Relics/BuilderRing.png";
    // 轮廓图标（原版85x85）
    // protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/BuilderRing.png";
    // 大图标（原版256x256）
    // protected override string BigIconPath => $"res://Diceomancer/images/Relics/BuilderRing_big.png";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HealVar(30)
    ];

    // 战斗开始前
    public override async Task BeforeCombatStart()
    {
        Flash();
        await CreatureCmd.SetCurrentHp(Owner.Creature, DynamicVars.Heal.BaseValue);
    }
}