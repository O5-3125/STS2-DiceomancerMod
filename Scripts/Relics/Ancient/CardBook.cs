using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using STS2RitsuLib;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Ancient;

[RegisterRelic(typeof(SharedRelicPool))]
public class CardBook : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    private int _cardCount = 999999;

    [SavedProperty]
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
        if (card.Pile?.Type is not PileType.Deck) return;
        if (Owner.Deck.Cards.Count <= CardCount) return;

        var cardModels = PileType.Deck.GetPile(Owner).Cards.Where(model => model.Type == CardType.Curse).ToList();

        if (cardModels.Count == 0)
        {
            cardModels = cardModels.Concat(
                PileType.Deck.GetPile(Owner).Cards.Where(model => model.Rarity == CardRarity.Basic).ToList()
            ).ToList();
        }

        if (cardModels.Count == 0)
        {
            cardModels = cardModels.Concat(
                PileType.Deck.GetPile(Owner).Cards.ToList()
            ).ToList();
        }

        var removeCard = Owner.RunState.Rng.Niche.NextItem(cardModels);

        if (removeCard != null)
        {
            Flash();
            await CardPileCmd.RemoveFromDeck(removeCard);
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


    // public override async Task AfterCombatEnd(CombatRoom room)
    // {
    //     if (!base.Owner.Creature.IsDead)
    //     {
    //         Flash();
    //         var cardModels = PileType.Deck.GetPile(Owner).Cards.ToList();
    //         
    //         if (cardModels.Count > CardCount)
    //         {
    //             var removeCards = (await CardSelectCmd.FromDeckForRemoval(
    //                     prefs: new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt,
    //                         cardModels.Count - CardCount),
    //                     // 1),
    //                     player: base.Owner))
    //                 // .Where(model => model.Keywords.Contains(CardKeyword.Eternal))
    //                 .ToList();
    //
    //             await CardPileCmd.RemoveFromDeck(removeCards);
    //         }
    //     }
    // }


    public override string PackedIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string BigIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
}