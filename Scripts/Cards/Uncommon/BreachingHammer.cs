using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class BreachingHammer()
    : ModCardTemplate(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(15, ValueProp.Move),
        new("multiples", 3)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        if (cardPlay.Target.Block > 0) DynamicVars.Damage.BaseValue *= DynamicVars["multiples"].IntValue;
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue) 
            .FromCard(this,cardPlay) 
            .Targeting(cardPlay.Target) 
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["multiples"].UpgradeValueBy(1); 
    }
}