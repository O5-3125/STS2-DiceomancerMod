using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Rare;

// [RegisterRelic(typeof(DiceomancerRelicPool))]
public class MyDeck : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    // 消耗牌时将其移出牌组
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card,
        bool causedByEthereal)
    {
        // if (!card.IsRemovable) return;

        Flash();

        if (PileType.Deck.GetPile(Owner).Cards.Contains(card.DeckVersion))
            await CardPileCmd.RemoveFromDeck(card.DeckVersion);
    }

    // 战斗中生成牌时将其加入牌组
    public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (creator != Owner) return;

        Flash();

        var cardModel = Owner.RunState.CloneCard(card);

        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(cardModel, PileType.Deck));
    }
}