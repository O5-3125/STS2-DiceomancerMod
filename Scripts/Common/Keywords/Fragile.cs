using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Diceomancer.Scripts.Common.Keywords;

internal static class FragileKeywordRegistration
{
    [RegisterOwnedCardKeyword("Fragile",
        CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
    private sealed class FragileKeyword;
}

public static class Fragile
{
    public static string FragileKeywordId =>
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, "Fragile");

    private static bool HasFragile(CardModel? card)
    {
        return card != null && card.Keywords.Contains(MyKeywords.Fragile);
    }

    private static async Task TriggerFragileEffect(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (PileType.Deck.GetPile(card.Owner).Cards.Contains(card.DeckVersion))
            await CardPileCmd.RemoveFromDeck(card.DeckVersion);

        await CardPileCmd.RemoveFromCombat(card);
    }

    [RegisterSingleton]
    public sealed class FragileSingleton : SingletonModel
    {
        public FragileSingleton()
        {
            ModHelper.SubscribeForCombatStateHooks(Id.Entry, CombatSubModels);
        }

        public override bool ShouldReceiveCombatHooks => true;

        private IEnumerable<AbstractModel> CombatSubModels(CombatState _)
        {
            return [this];
        }

        public override Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (!HasFragile(cardPlay.Card)) return Task.CompletedTask;
            return TriggerFragileEffect(choiceContext, cardPlay.Card);
        }
    }
}