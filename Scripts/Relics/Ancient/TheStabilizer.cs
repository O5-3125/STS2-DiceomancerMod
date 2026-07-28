using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Ancient;

[RegisterRelic(typeof(EventRelicPool))]
public class TheStabilizer : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private int _cardCount = 999999;

    private int CardCount
    {
        get => _cardCount;
        set
        {
            AssertMutable();
            _cardCount = value;
            InvokeDisplayAmountChanged();
        }
    }

    public override bool ShowCounter => true;
    public override int DisplayAmount => CardCount;

    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card.Pile is not { Type: PileType.Deck }) return;

        var cardModels = PileType.Deck.GetPile(Owner).Cards.ToList();

        Flash();
        if (cardModels.Count > CardCount)
        {
            var removeCards = (await CardSelectCmd.FromDeckForRemoval(
                    prefs: new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt,
                        cardModels.Count - CardCount),
                    // 1),
                    player: base.Owner))
                // .Where(model => model.Keywords.Contains(CardKeyword.Eternal))
                .ToList();

            await CardPileCmd.RemoveFromDeck(removeCards);
        }
    }

    public override Task AfterObtained()
    {
        CardCount
            = PileType.Deck.GetPile(Owner).Cards
                .Count
            // (model => model.Keywords.Contains(CardKeyword.Eternal))
            ;
        return Task.CompletedTask;
    }

    public override string PackedIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string BigIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
}