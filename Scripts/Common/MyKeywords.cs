using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Diceomancer.Scripts.Common;

//CardDescriptionPlacement代表这个关键词的描述加在卡牌的位置。默认不显示。
// [RegisterOwnedCardKeyword(nameof(Diabolical),
//     CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
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


    public static readonly CardKeyword Chaos4 =
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Chaos4)).GetModCardKeyword();

    public static readonly CardKeyword Chaos6 =
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Chaos6)).GetModCardKeyword();

    public static readonly CardKeyword Chaos8 =
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Chaos8)).GetModCardKeyword();

    public static readonly CardKeyword Chaos12 =
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Chaos12)).GetModCardKeyword();

    public static readonly CardKeyword Chaos20 =
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Chaos20)).GetModCardKeyword();

    public static readonly CardKeyword Diabolical =
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, nameof(Diabolical)).GetModCardKeyword();
    
    
}