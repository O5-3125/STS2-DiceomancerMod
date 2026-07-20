using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Rare;

// [RegisterCard(typeof(DiceomancerCardPool))]
public class Explosion() : ModCardTemplate(1, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);

        IEnumerable<Creature> targets = this.CombatState.Creatures;
        var amount = 0;
        foreach (var target in targets)
        {
            amount += target.GetPowerAmount<BurnPower>();
            await PowerCmd.Remove<BurnPower>(target);
        }
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this,cardPlay)
            .TargetingAllOpponents(this.CombatState)
            .WithHitCount(amount)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}