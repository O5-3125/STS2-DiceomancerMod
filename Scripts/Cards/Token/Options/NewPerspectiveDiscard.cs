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
public class NewPerspectiveDiscard()
    : OptionsTemplate()
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var handPile = PileType.Hand.GetPile(Owner).Cards.ToList();
        var discardPile = PileType.Discard.GetPile(Owner).Cards.ToList();
        await CardPileCmd.Add(handPile, PileType.Discard);
        await CardPileCmd.Add(discardPile, PileType.Hand);
    }
}