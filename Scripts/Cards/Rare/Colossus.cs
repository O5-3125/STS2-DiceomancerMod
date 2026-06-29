using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Rare;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Colossus() : ModCardTemplate(4, CardType.Power, CardRarity.Rare, TargetType.Self, true)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<StrengthPower>(16),
        new PowerVar<ThickSkin>(8),
        new PowerVar<Powers.Colossus>(3)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<StrengthPower>(choiceContext, base.Owner.Creature,
            DynamicVars["StrengthPower"].IntValue, base.Owner.Creature, this);
        await PowerCmd.Apply<ThickSkin>(choiceContext, base.Owner.Creature,
            DynamicVars["ThickSkin"].IntValue, base.Owner.Creature, this);
        await PowerCmd.Apply<Powers.Colossus>(choiceContext, base.Owner.Creature,
            DynamicVars["Colossus"].IntValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}