using Diceomancer.Scripts.Common.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Diceomancer.Scripts.Common.Keywords;

internal static class ChaosNKeywordRegistration
{
    [RegisterOwnedCardKeyword("Chaos4",
        CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
    private sealed class Chaos4Keyword;

    [RegisterOwnedCardKeyword("Chaos6",
        CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
    private sealed class Chaos6Keyword;

    [RegisterOwnedCardKeyword("Chaos8",
        CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
    private sealed class Chaos8Keyword;

    [RegisterOwnedCardKeyword("Chaos12",
        CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
    private sealed class Chaos12Keyword;

    [RegisterOwnedCardKeyword("Chaos20",
        CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
    private sealed class Chaos20Keyword;
}

public static class ChaosN
{
    public static string Chaos4KeywordId =>
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, "Chaos4");

    public static string Chaos6KeywordId =>
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, "Chaos6");

    public static string Chaos8KeywordId =>
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, "Chaos8");

    public static string Chaos12KeywordId =>
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, "Chaos12");

    public static string Chaos20KeywordId =>
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, "Chaos20");

    private static bool HasChaosN(CardModel? card)
    {
        if (card == null) return false;
        return card.Keywords.Contains(MyKeywords.Chaos4)
            || card.Keywords.Contains(MyKeywords.Chaos6)
            || card.Keywords.Contains(MyKeywords.Chaos8)
            || card.Keywords.Contains(MyKeywords.Chaos12)
            || card.Keywords.Contains(MyKeywords.Chaos20);
    }

    private static int RollChaosDie(CardModel card)
    {
        if (card.Keywords.Contains(MyKeywords.Chaos4)) return RandomCmd.CheckD4(card.Owner);
        if (card.Keywords.Contains(MyKeywords.Chaos6)) return RandomCmd.CheckD6(card.Owner);
        if (card.Keywords.Contains(MyKeywords.Chaos8)) return RandomCmd.CheckD8(card.Owner);
        if (card.Keywords.Contains(MyKeywords.Chaos12)) return RandomCmd.CheckD12(card.Owner);
        if (card.Keywords.Contains(MyKeywords.Chaos20)) return RandomCmd.CheckD20(card.Owner);
        return 0;
    }

    private static Task TriggerChaosEffect(PlayerChoiceContext choiceContext, CardModel card)
    {
        ModifyCardCmd.ModifyCardDynamicVars(card, RollChaosDie(card));
        return Task.CompletedTask;
    }

    [RegisterSingleton]
    public sealed class ChaosNSingleton : SingletonModel
    {
        public ChaosNSingleton()
        {
            ModHelper.SubscribeForCombatStateHooks(Id.Entry, CombatSubModels);
        }

        public override bool ShouldReceiveCombatHooks => true;

        private IEnumerable<AbstractModel> CombatSubModels(CombatState _)
        {
            return [this];
        }

        public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
        {
            if (!HasChaosN(card)) return Task.CompletedTask;
            return TriggerChaosEffect(choiceContext, card);
        }
    }
}
