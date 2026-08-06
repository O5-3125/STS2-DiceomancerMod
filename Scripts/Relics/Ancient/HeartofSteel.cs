using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Ancient;

[RegisterRelic(typeof(SharedRelicPool))]
public class HeartOfSteel : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override string PackedIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string BigIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";

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