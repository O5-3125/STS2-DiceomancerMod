using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Rare;

[RegisterCard(typeof(DiceomancerCardPool))]
public class UnimaginablePath() : ModCardTemplate(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
        MyKeywords.Chaos
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(6)
    ];

    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardModels =
            (await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt,
                    0, base.DynamicVars.Cards.IntValue),
                context: choiceContext, player: base.Owner, filter: null, source: this)).ToList();

        foreach (var cardModel in cardModels)
        {
            if (PileType.Deck.GetPile(Owner).Cards.Contains(cardModel.DeckVersion))
            {
                var newCard = await CardCmd.TransformToRandom(cardModel, Owner.RunState.Rng.CombatCardGeneration);
                var newCardDeck = Owner.RunState.CloneCard(newCard.cardAdded);
                await CardCmd.Transform(cardModel.DeckVersion, newCardDeck);
            }
            else
            {
                var newCard = await CardCmd.TransformToRandom(cardModel, Owner.RunState.Rng.CombatCardSelection);
                var newCardDeck = Owner.RunState.CloneCard(newCard.cardAdded);
                CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(newCardDeck, PileType.Deck));
            }
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}