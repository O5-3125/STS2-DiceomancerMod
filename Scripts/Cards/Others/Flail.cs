using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Common;

// [RegisterCard(typeof(DiceomancerCardPool))]
public class Flail() : ModCardTemplate(0, CardType.Attack, CardRarity.Common, TargetType.RandomEnemy, true)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6, ValueProp.Move),
        new("SelfDamage", 3)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Retain,
        MyKeywords.Bonus
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars["SelfDamage"].IntValue) 
            .FromCard(this,cardPlay)
            .Targeting(this.Owner.Creature) 
            .Execute(choiceContext);

        ArgumentNullException.ThrowIfNull(this.CombatState, "this.CombatState");
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue) 
            .FromCard(this,cardPlay)
            .TargetingRandomOpponents(this.CombatState) 
            .Execute(choiceContext);
    }


    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(4);
    }
}