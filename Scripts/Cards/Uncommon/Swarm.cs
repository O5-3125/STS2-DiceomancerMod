using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Swarm() : ModCardTemplate(energyCost, type, rarity, targetType)
{
    // 基础耗能
    private const int energyCost = 2;

    // 卡牌类型
    private const CardType type = CardType.Attack;

    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Uncommon;

    // 目标类型（AnyEnemy表示任意敌人�?
    private const TargetType targetType = TargetType.RandomEnemy;

    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;


    protected override HashSet<CardTag> CanonicalTags =>
    [
        MyTags.Evolution.GetModCardTag()
    ];

    // 卡牌的基础属性（例如这里�?2点伤害）

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3m, ValueProp.Move),
        new RepeatVar(3),
        new DynamicVar("Evolution", 2M)
            .WithSharedTooltip("Evolution")
    ];

    // 添加这一行，指定卡牌立绘路径，这里是MyMod/images/cards/Test.png
    // public override string PortraitPath => $"res://MyMod/images/cards/{nameof(Test)}.png";

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState, "base.CombatState");

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue) // 造成伤害，数值来源于卡牌的基础伤害属�?
            .FromCard(this) // 伤害来源于这张卡�?
            .TargetingRandomOpponents(CombatState) // 随机选择目标
            .WithHitCount(DynamicVars.Repeat.IntValue) // 攻击次数
            .Execute(choiceContext);
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars["Evolution"].UpgradeValueBy(1);
        // DynamicVars.Damage.UpgradeValueBy(4); // 升级后增�?点伤�?
    }
}