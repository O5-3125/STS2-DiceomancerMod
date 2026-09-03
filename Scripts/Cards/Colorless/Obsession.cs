using Diceomancer.Scripts.Common.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Colorless;

[RegisterCard(typeof(ColorlessCardPool))]
public class Obsession() : ModCardTemplate(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Exhaust, MyKeywords.Limited];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(4)
        // new CalculatedDamageVar(ValueProp.Move).WithMultiplier((model, creature) => ResolveEnergyXValue)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<CardModel> handCards;
        CardModel? card;
        do
        {
            card = await CardPileCmd.Draw(choiceContext, base.Owner);
            handCards = PileType.Hand.GetPile(Owner).Cards.ToList();
        } while (card != null &&
                 handCards.Count < DynamicVars.Cards.IntValue &&
                 handCards.Count < CardPile.MaxCardsInHand &&
                 !CardPile.GetCards(base.Owner, PileType.Draw).Any());
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(2);
    }
}