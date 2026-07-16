using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
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
        $"res://Diceomancer/images/Power/{GetType().Name}.png"
    );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var array =
            (await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt,
                    0, base.DynamicVars.Cards.IntValue),
                context: choiceContext, player: base.Owner, filter: null, source: this)).ToArray();

        foreach (var cardModel in array)
        {
            if (PileType.Deck.GetPile(Owner).Cards.Contains(cardModel.DeckVersion))
            {
                var newCard = await CardCmd.TransformToRandom(cardModel, Owner.RunState.Rng.CombatCardGeneration);
                await CardCmd.Transform(cardModel.DeckVersion, newCard.cardAdded);
            }
            else
            {
                var newCard = await CardCmd.TransformToRandom(cardModel, Owner.RunState.Rng.CombatCardSelection);

                CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(newCard.cardAdded, PileType.Deck));
            }
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}