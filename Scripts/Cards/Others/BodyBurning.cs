using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Uncommon;

// [RegisterCard(typeof(DiceomancerCardPool))]
public class BodyBurning() : ModCardTemplate(2, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<Powers.BodyBurning>(3)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<Powers.BodyBurning>(choiceContext, this.Owner.Creature,
            DynamicVars["BodyBurning"].IntValue, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["BodyBurning"].UpgradeValueBy(1);
    }
}