using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Ancient;

// 加入哪个卡池
[RegisterCard(typeof(CurseCardPool))]
public class NullCard()
    : ModCardTemplate(energyCost, type, rarity, targetType)
{
    private const int energyCost = -1;

    private const CardType type = CardType.Curse;
    
    private const CardRarity rarity = CardRarity.Ancient;

    private const TargetType targetType = TargetType.Self;

    private const bool shouldShowInCardLibrary = true;

    public override int MaxUpgradeLevel => 0;


    // protected override HashSet<CardTag> CanonicalTags =>
    // [
    //     MyTags.Evolution.GetModCardTag(),
    //     MyTags.Modify.GetModCardTag()
    // ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        MyKeywords.Bonus, // 添加自定义关键词
        CardKeyword.Unplayable // 添加原版关键
    ];


    // // 通过HoverTipFactory添加各种提示文本
    // protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    // [
    //     // HoverTipFactory.FromCard<Flame>(),
    //     // HoverTipFactory.FromPower<BurnPower>(),
    //     // HoverTipFactory.FromKeyword(CardKeyword.Unplayable),
    //     // ModKeywordRegistry.CreateHoverTip(MyKeywords.Rebound), // 自定义关键词
    // ];

    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );
}