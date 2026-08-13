using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Diceomancer.Scripts.Common.Keywords;

internal static class WildKeywordRegistration
{
    [RegisterOwnedCardKeyword("Wild",
        CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
    private sealed class WildKeyword;
}

public static class Wild
{
    public static string WildKeywordId =>
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, "Wild");

    public static bool HasWild(CardModel? card)
    {
        return card != null && card.Keywords.Contains(MyKeywords.Wild);
    }
}
