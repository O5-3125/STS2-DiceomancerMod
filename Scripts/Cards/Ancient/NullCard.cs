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
    // 基础耗能
    private const int energyCost = -1;

    // 卡牌类型
    private const CardType type = CardType.Curse;

    // 卡牌稀有度
    // Ancient Basic Common None Curse Event Quest Rare Status Token Uncommon
    // 先古 基础 常见 没有 诅咒 事件 任务 稀�?状�?任务 罕见
    private const CardRarity rarity = CardRarity.Ancient;

    // 目标类型
    private const TargetType targetType = TargetType.Self;

    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;

    // 不能被升�?
    public override int MaxUpgradeLevel => 0;


    // protected override HashSet<CardTag> CanonicalTags =>
    // [
    //     MyTags.Evolution.GetModCardTag(),
    //     MyTags.Modify.GetModCardTag()
    // ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        MyKeywords.Bonus, // 添加自定义关键词
        CardKeyword.Unplayable // 添加原版关键�?
    ];


    // // 通过HoverTipFactory添加各种提示文本
    // protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    // [
    //     // HoverTipFactory.FromCard<Flame>(),
    //     // HoverTipFactory.FromPower<BurnPower>(),
    //     // HoverTipFactory.FromKeyword(CardKeyword.Unplayable),
    //     // ModKeywordRegistry.CreateHoverTip(MyKeywords.Rebound), // 自定义关键词
    // ];

    // 自定义卡�?
    public override CardAssetProfile AssetProfile => new(
        "res://Diceomancer/images/Cards/占位�?.png" // 卡图
    );
}