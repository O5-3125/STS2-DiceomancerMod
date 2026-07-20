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
public class Swarm() : ModCardTemplate(2, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override HashSet<CardTag> CanonicalTags =>
    [
        MyTags.Evolution.GetModCardTag()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3m, ValueProp.Move),
        new RepeatVar(3),
        new DynamicVar("Evolution", 2M)
            .WithSharedTooltip("evolution")
    ];

    // 添加这一行，指定卡牌立绘路径，这里是MyMod/images/cards/Test.png
    // public override string PortraitPath => $"res://MyMod/images/cards/{nameof(Test)}.png";

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState, "base.CombatState");

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay) // 伤害来源于这张卡?
            .TargetingRandomOpponents(CombatState) // 随机选择目标
            .WithHitCount(DynamicVars.Repeat.IntValue) // 攻击次数
            .Execute(choiceContext);
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars["Evolution"].UpgradeValueBy(1);
        // DynamicVars.Damage.UpgradeValueBy(4); 
    }
}