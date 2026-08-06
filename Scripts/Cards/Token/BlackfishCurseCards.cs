using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Void = MegaCrit.Sts2.Core.Models.Cards.Void;

namespace Diceomancer.Scripts.Cards.Token;

// 黑鱼诅咒的可选内容：作为卡牌展示给玩家选择，被选择后执行对应效果
public interface IBlackfishCurse
{
    Task OnChosen();
}

public abstract class BlackfishCurseCard()
    : ModCardTemplate(-1, CardType.Status, CardRarity.Status, TargetType.None), IBlackfishCurse
{
    public override bool CanBeGeneratedInCombat => false;

    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public abstract Task OnChosen();

    // 施法者默认是玩家自己（参考游戏本体知识恶魔的诅咒牌）
    protected static ThrowingPlayerChoiceContext ChoiceContext => new();

    protected IEnumerable<Creature> EnemyCreatures =>
        Owner.Creature.CombatState.Creatures.Where(c => !c.IsDead && c.Side == CombatSide.Enemy);
}

// 诅咒1：对自己造成6点伤害（可被格挡）
[RegisterCard(typeof(TokenCardPool))]
public class CurseSelfDamage : BlackfishCurseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6, ValueProp.Unpowered | ValueProp.Move)
    ];

    public override async Task OnChosen()
    {
        await CreatureCmd.Damage(ChoiceContext, Owner.Creature, DynamicVars.Damage.BaseValue,
            ValueProp.Unpowered | ValueProp.Move, this, null);
    }
}

// 诅咒2：失去5点生命
[RegisterCard(typeof(TokenCardPool))]
public class CurseLoseHp : BlackfishCurseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(5)
    ];

    public override async Task OnChosen()
    {
        await CreatureCmd.Damage(ChoiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, null);
    }
}

// 诅咒3：失去3点最大生命值
[RegisterCard(typeof(TokenCardPool))]
public class CurseLoseMaxHp : BlackfishCurseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new MaxHpVar(3)
    ];

    public override async Task OnChosen()
    {
        await CreatureCmd.LoseMaxHp(ChoiceContext, Owner.Creature, DynamicVars.MaxHp.BaseValue, true);
    }
}

// 诅咒4：弃随机手牌
[RegisterCard(typeof(TokenCardPool))]
public class CurseDiscardCards : BlackfishCurseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1)
    ];

    public override async Task OnChosen()
    {
        var hand = PileType.Hand.GetPile(Owner).Cards.ToList();
        var cardModel = Owner.RunState.Rng.CombatCardSelection.NextItem(hand);
        if (cardModel != null)
            await CardCmd.Discard(ChoiceContext, cardModel);
    }
}

// 诅咒5：对自身施加6层无力
[RegisterCard(typeof(TokenCardPool))]
public class CurseFrail : BlackfishCurseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<FrailPower>(6)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<FrailPower>()
    ];

    public override async Task OnChosen()
    {
        await PowerCmd.Apply<FrailPower>(ChoiceContext, Owner.Creature, DynamicVars["FrailPower"].IntValue,
            Owner.Creature, this);
    }
}

// 诅咒6：对自身施加2层虚弱
[RegisterCard(typeof(TokenCardPool))]
public class CurseWeak : BlackfishCurseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<WeakPower>(2)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<WeakPower>()
    ];

    public override async Task OnChosen()
    {
        await PowerCmd.Apply<WeakPower>(ChoiceContext, Owner.Creature, DynamicVars["WeakPower"].IntValue,
            Owner.Creature, this);
    }
}

// 诅咒7：对自身施加2层易伤
[RegisterCard(typeof(TokenCardPool))]
public class CurseVulnerable : BlackfishCurseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<VulnerablePower>(2)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<VulnerablePower>()
    ];

    public override async Task OnChosen()
    {
        await PowerCmd.Apply<VulnerablePower>(ChoiceContext, Owner.Creature, DynamicVars["VulnerablePower"].IntValue,
            Owner.Creature, this);
    }
}

// 诅咒8：对自身施加3层慌乱
[RegisterCard(typeof(TokenCardPool))]
public class CursePanic : BlackfishCurseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<PanicPower>(3)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<PanicPower>()
    ];

    public override async Task OnChosen()
    {
        await PowerCmd.Apply<PanicPower>(ChoiceContext, Owner.Creature, DynamicVars["PanicPower"].IntValue,
            Owner.Creature, this);
    }
}

// 诅咒9：对自身施加3层负担
[RegisterCard(typeof(TokenCardPool))]
public class CurseSlow : BlackfishCurseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<BurdenPower>(3)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<BurdenPower>()
    ];

    public override async Task OnChosen()
    {
        await PowerCmd.Apply<BurdenPower>(ChoiceContext, Owner.Creature, DynamicVars["BurdenPower"].IntValue,
            Owner.Creature, this);
    }
}

// 诅咒10：对自身施加1层目盲
[RegisterCard(typeof(TokenCardPool))]
public class CurseBlind : BlackfishCurseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<BlindPower>(1)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<BlindPower>()
    ];

    public override async Task OnChosen()
    {
        await PowerCmd.Apply<BlindPower>(ChoiceContext, Owner.Creature, DynamicVars["BlindPower"].IntValue,
            Owner.Creature, this);
    }
}

// 诅咒11：加5张「虚空」到抽牌堆
[RegisterCard(typeof(TokenCardPool))]
public class CurseVoidCards : BlackfishCurseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(5)
    ];

    public override async Task OnChosen()
    {
        await CardPileCmd.AddToCombatAndPreview<Void>(Owner.Creature, PileType.Draw, DynamicVars.Cards.IntValue, null,
            CardPilePosition.Random);
    }
}

// 诅咒12：加3张「粘液」到手牌
[RegisterCard(typeof(TokenCardPool))]
public class CurseSlimedCards : BlackfishCurseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3)
    ];

    public override async Task OnChosen()
    {
        await CardPileCmd.AddToCombatAndPreview<Slimed>(Owner.Creature, PileType.Draw, DynamicVars.Cards.IntValue, null,
            CardPilePosition.Random);
    }
}

// 诅咒13：所有敌人获得3层力量
[RegisterCard(typeof(TokenCardPool))]
public class CurseEnemyStrength : BlackfishCurseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<StrengthPower>(3)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<StrengthPower>()
    ];

    public override async Task OnChosen()
    {
        await PowerCmd.Apply<StrengthPower>(ChoiceContext, base.CombatState?.HittableEnemies,
            DynamicVars["StrengthPower"].IntValue, Owner.Creature, this);
    }
}

// 诅咒14：所有敌人获得10层加固
[RegisterCard(typeof(TokenCardPool))]
public class CurseEnemyFortify : BlackfishCurseCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<FortifiedPower>(10)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<FortifiedPower>()
    ];

    public override async Task OnChosen()
    {
        await PowerCmd.Apply<FortifiedPower>(ChoiceContext, base.CombatState?.HittableEnemies,
            DynamicVars["FortifiedPower"].IntValue, Owner.Creature, this);
    }
}
