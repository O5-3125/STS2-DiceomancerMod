using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Hero.CardPool;

namespace Diceomancer.Scripts.Cards.Token.Options;

[RegisterCard(typeof(OptionsCardPool))]
public class EvolutionPlating()
    : OptionsTemplate()
{

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<FortifiedPower>(3)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<FortifiedPower>(choiceContext, Owner.Creature,
            DynamicVars["FortifiedPower"].IntValue, Owner.Creature, this);
    }
}