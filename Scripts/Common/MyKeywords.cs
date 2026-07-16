using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Diceomancer.Scripts.Common;

//CardDescriptionPlacement代表这个关键词的描述加在卡牌的位置。默认不显示。
[RegisterOwnedCardKeyword(nameof(Bonus),
    CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(Limited),
    CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
[RegisterOwnedCardKeyword(nameof(Rebound),
    CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
[RegisterOwnedCardKeyword(nameof(Fragile),
    CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
[RegisterOwnedCardKeyword(nameof(Phantom),
    CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
[RegisterOwnedCardKeyword(nameof(Chaos),
    CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
[RegisterOwnedCardKeyword(nameof(Epidemic),
    CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
public class MyKeywords
{
    public static readonly CardKeyword Bonus = ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Bonus))
        .GetModCardKeyword();

    public static readonly CardKeyword Rebound =
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Rebound)).GetModCardKeyword();

    public static readonly CardKeyword Fragile =
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Fragile)).GetModCardKeyword();

    public static readonly CardKeyword Limited =
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Limited)).GetModCardKeyword();

    public static readonly CardKeyword Phantom =
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Phantom)).GetModCardKeyword();

    public static readonly CardKeyword Chaos =
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Chaos)).GetModCardKeyword();

    public static readonly CardKeyword Epidemic =
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Epidemic)).GetModCardKeyword();

    
}