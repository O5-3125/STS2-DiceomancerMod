using Diceomancer.Scripts.Powers.NormalityPower;
using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Godot;
using STS2RitsuLib.Scaffolding.Visuals.StateMachine;

namespace Diceomancer.Scripts.Monsters;

// 树哥：一层强怪
// 三种行为模式（格挡拳、火焰拳、双拳）按顺序循环，战斗开始时一定是格挡拳。
// 每轮循环后所有意图的基础伤害+1。
[RegisterMonster]
public class TreeBro : ModMonsterTemplate
{
    // 低进阶65血，高进阶70血
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 70, 65);

    public override int MaxInitialHp => MinInitialHp;

    // 已完成的循环轮数，每轮循环后所有意图的基础伤害+1
    private int _cycleCount;

    private int CycleCount
    {
        get => _cycleCount;
        set
        {
            AssertMutable();
            _cycleCount = value;
        }
    }

    // 基础伤害，随循环轮数增长
    private int BaseDamage => 6 + CycleCount;

    // 格挡拳获得的格挡
    private const int BlockAmount = 5;

    // 火焰拳施加的燃烧层数
    private const int BurnAmount = 3;

    public override MonsterAssetProfile AssetProfile => new(
        "res://Diceomancer/scenes/Monsters/woody/woody.tscn"
    );

    public override DamageSfxType TakeDamageSfxType =>  DamageSfxType.Armor;

    // 自动转换怪物场景，让你不需要手动挂脚本。复制即可。
    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.VisualsScenePath!);
    }

    // 帧动画状态机：把游戏的动画触发器(Idle/Attack/Hit/Dead...)路由到 woody.tscn 里的 AnimatedSprite2D
    protected override ModAnimStateMachine? SetupCustomCombatAnimationStateMachine(Node visualsRoot, MonsterModel monster)
    {
        return ModAnimStateMachines.StandardCue(
            visualsRoot,
            character: null,
            idleName: "idle",
            deadName: null, // 暂无死亡序列帧，死亡时退回待机
            deadLoop: false,
            hitName: "hurt",
            hitLoop: false,
            attackName: "attack",
            attackLoop: false,
            castName: null,
            castLoop: false,
            relaxedName: null,
            relaxedLoop: true);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        // 意图1：格挡拳，造成伤害，获得格挡
        var blockFist = new MoveState(
            "BLOCK_FIST", // 状态ID
            BlockFistMove, // 执行函数
            new SingleAttackIntent(() => BaseDamage),
            new DefendIntent()
        );

        // 意图2：火焰拳，造成伤害，施加燃烧
        var flameFist = new MoveState(
            "FLAME_FIST",
            FlameFistMove,
            new SingleAttackIntent(() => BaseDamage),
            new DebuffIntent()
        );

        // 意图3：双拳，造成伤害2次
        var doubleFist = new MoveState(
            "DOUBLE_FIST",
            DoubleFistMove,
            new ScalingMultiAttackIntent(() => BaseDamage, 2)
        );

        // 战斗开始时一定是格挡拳，三者按顺序循环
        blockFist.FollowUpState = flameFist;
        flameFist.FollowUpState = doubleFist;
        doubleFist.FollowUpState = blockFist;

        return new MonsterMoveStateMachine([blockFist, flameFist, doubleFist], blockFist);
    }


    // 意图1执行实际效果：造成伤害，获得格挡
    private async Task BlockFistMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(BaseDamage)
            .FromMonster(this)
            .WithAttackerFx(null, AttackSfx) // 攻击动画由状态机触发器自动播放
            .WithHitFx("vfx/vfx_attack_blunt") // 命中特效
            .Execute(null);

        await CreatureCmd.GainBlock(Creature, BlockAmount, ValueProp.Move, null);
    }

    // 意图2执行实际效果：造成伤害，施加燃烧
    private async Task FlameFistMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(BaseDamage)
            .FromMonster(this)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
        await PowerCmd.Apply<BurnPower>(new ThrowingPlayerChoiceContext(), targets, BurnAmount, Creature, null);
    }

    // 意图3执行实际效果：造成伤害2次，本轮循环结束，基础伤害+1
    private async Task DoubleFistMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(BaseDamage)
            .WithHitCount(2)
            .FromMonster(this)
            .OnlyPlayAnimOnce()
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);

        CycleCount++;
    }

    // MultiAttackIntent 的伤害在构造时固定，无法动态变化，所以包一层支持动态伤害
    private sealed class ScalingMultiAttackIntent : MultiAttackIntent
    {
        public ScalingMultiAttackIntent(Func<int> damageCalc, int repeat)
            : base(0, repeat)
        {
            DamageCalc = () => damageCalc();
        }
    }
}