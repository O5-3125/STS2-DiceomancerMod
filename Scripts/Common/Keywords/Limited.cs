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

internal static class LimitedKeywordRegistration
{
    [RegisterOwnedCardKeyword("Limited",
        CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
    private sealed class LimitedKeyword;
}

public static class Limited
{
    public static string LimitedKeywordId =>
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, "Limited");

    private static bool HasLimited(CardModel? card)
    {
        return card != null && card.Keywords.Contains(MyKeywords.Limited);
    }

    private static async Task TriggerLimitedEffect(PlayerChoiceContext choiceContext, CardModel card)
    {
        await PowerCmd.Apply<Fatigue>(choiceContext, card.Owner.Creature, 1, null, null);
    }

    [RegisterSingleton]
    public sealed class LimitedSingleton : SingletonModel
    {
        public LimitedSingleton()
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
            if (!HasLimited(cardPlay.Card)) return Task.CompletedTask;
            return TriggerLimitedEffect(choiceContext, cardPlay.Card);
        }
    }
}