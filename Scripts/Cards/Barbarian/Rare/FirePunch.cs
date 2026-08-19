using Diceomancer.Scripts.Hero.Barbarian;
using Diceomancer.Scripts.Powers.Berserker;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Barbarian.Rare;

[RegisterCard(typeof(BarbarianCardPool))]
public class FirePunch() : ModCardTemplate(1, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<BurnPower>(12)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);

        var enemies = CombatState.HittableEnemies.ToList();

        await PowerCmd.Apply<BurnPower>(choiceContext, enemies,
            DynamicVars["BurnPower"].IntValue, Owner.Creature, this);

        foreach (var enemy in enemies.Where(e => !e.IsDead))
            await PowerCmd.Apply<FirePunchPower>(choiceContext, enemy, 1, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}