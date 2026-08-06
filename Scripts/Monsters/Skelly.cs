using Diceomancer.Scripts.Powers.MonstersPower;
using Godot;
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
using STS2RitsuLib.Scaffolding.Visuals.StateMachine.Backends;

namespace Diceomancer.Scripts.Monsters;

// 骷髅哥：密林小怪
// 初始拥有能力模块化脑瓜子，战斗开始处于甩鞭子，然后甩鞭子、动脑瓜子两种模式交替。
// 决定意图时有无脑瓜子会产生不一样的意图。
// 有脑瓜子的情况下失去脑瓜子（半血触发或动脑瓜子），当前行动模式会切换至甩鞭子，意图变为没脑瓜子的甩鞭子。
[RegisterMonster]
public class Skelly : ModMonsterTemplate
{
    // 低进阶72血，高进阶80血
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 80, 72);

    public override int MaxInitialHp => MinInitialHp;

    // 甩鞭子：有脑瓜子造成12伤害，没脑瓜子造成6伤害
    private const int WhipBrainDamage = 12;

    private const int WhipNoBrainDamage = 6;

    // 动脑瓜子：有脑瓜子造成13伤害并失去脑瓜子；没脑瓜子获得12点格挡并获得脑瓜子
    private const int BrainDamage = 13;

    private const int BlockAmount = 12;

    // 是否有脑瓜子
    private bool HasBrain => Creature.HasPower<SkellyBrainPower>();

    // 怪物场景
    public override MonsterAssetProfile AssetProfile => new(
        "res://Diceomancer/scenes/Monsters/skelly/skelly.tscn"
    );

    // 自动转换怪物场景，让你不需要手动挂脚本。复制即可。
    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.VisualsScenePath!);
    }

    // 战斗开始时，初始拥有能力模块化脑瓜子
    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();
        await PowerCmd.Apply<SkellyBrainPower>(new ThrowingPlayerChoiceContext(), Creature, 1m, Creature, null);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        // 甩鞭子（有脑瓜子）：造成12伤害
        var whipBrain = new MoveState(
            "WHIP_BRAIN", // 状态ID
            WhipBrainMove, // 执行函数
            new SingleAttackIntent(WhipBrainDamage)
        );

        // 甩鞭子（没脑瓜子）：造成6伤害
        var whipNoBrain = new MoveState(
            "WHIP_NO_BRAIN",
            WhipNoBrainMove,
            new SingleAttackIntent(WhipNoBrainDamage)
        );

        // 动脑瓜子（有脑瓜子）：造成13伤害，失去脑瓜子
        var brainAttack = new MoveState(
            "BRAIN_ATTACK",
            BrainAttackMove,
            new SingleAttackIntent(BrainDamage),new BuffIntent()
        );

        // 动脑瓜子（没脑瓜子）：获得12点格挡，获得脑瓜子
        var brainDefend = new MoveState(
            "BRAIN_DEFEND",
            BrainDefendMove,
            new DefendIntent(),new BuffIntent()
        );

        // 根据有无脑瓜子选择对应的意图
        var whipBranch = new ConditionalBranchState("WHIP_BRANCH");
        whipBranch.AddState(whipBrain, () => HasBrain);
        whipBranch.AddState(whipNoBrain, () => !HasBrain);

        var brainBranch = new ConditionalBranchState("BRAIN_BRANCH");
        brainBranch.AddState(brainAttack, () => HasBrain);
        brainBranch.AddState(brainDefend, () => !HasBrain);

        // 甩鞭子与动脑瓜子两种模式交替，战斗开始处于甩鞭子
        whipBrain.FollowUpState = brainBranch;
        whipNoBrain.FollowUpState = brainBranch;
        brainAttack.FollowUpState = whipBranch;
        brainDefend.FollowUpState = whipBranch;

        return new MonsterMoveStateMachine(
            [whipBranch, brainBranch, whipBrain, whipNoBrain, brainAttack, brainDefend],
            whipBranch);
    }

    // 有脑瓜子时失去脑瓜子：当前行动模式切换至甩鞭子，意图变为没脑瓜子的甩鞭子
    public void SwitchToWhipMode()
    {
        if (MoveStateMachine != null &&
            MoveStateMachine.States.TryGetValue("WHIP_NO_BRAIN", out var state) &&
            state is MoveState whip)
        {
            SetMoveImmediate(whip, forceTransition: true);
        }
    }

    // 脑瓜子形态变化后，立刻把动画切换到对应形态的 idle
    public async Task RefreshFormAnimation()
    {
        await CreatureCmd.TriggerAnim(Creature, "FormChange", 0f);
    }

    // 甩鞭子（有脑瓜子）执行实际效果：造成12伤害
    private async Task WhipBrainMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(WhipBrainDamage)
            .FromMonster(this)
            .WithAttackerAnim("Attack", 0.4f)
            .WithAttackerFx(null, AttackSfx) // 攻击音效
            .WithHitFx("vfx/vfx_attack_blunt") // 攻击特效
            .Execute(null);
    }

    // 甩鞭子（没脑瓜子）执行实际效果：造成6伤害
    private async Task WhipNoBrainMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(WhipNoBrainDamage)
            .FromMonster(this)
            .WithAttackerAnim("Attack", 0.4f)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
    }

    // 动脑瓜子（有脑瓜子）执行实际效果：造成13伤害，失去脑瓜子
    private async Task BrainAttackMove(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.TriggerAnim(Creature, "Special1", 0.5f);
        await DamageCmd
            .Attack(BrainDamage)
            .FromMonster(this)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
        // 失去脑瓜子
        await PowerCmd.Remove<SkellyBrainPower>(Creature);
        await RefreshFormAnimation();
    }

    // 动脑瓜子（没脑瓜子）执行实际效果：获得12点格挡，获得脑瓜子
    private async Task BrainDefendMove(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.TriggerAnim(Creature, "Special2", 0.5f);
        await CreatureCmd.GainBlock(Creature, BlockAmount, ValueProp.Move, null);
        // 获得脑瓜子
        await PowerCmd.Apply<SkellyBrainPower>(new ThrowingPlayerChoiceContext(), Creature, 1m, Creature, null);
        await RefreshFormAnimation();
    }

    // 骷髅哥是帧动画怪物，场景里有两套动画（形态1有脑瓜子 / 形态2没脑瓜子），把战斗触发映射到对应的动画
    protected override ModAnimStateMachine? SetupCustomCombatAnimationStateMachine(Node visualsRoot, MonsterModel monster)
    {
        var sprite = visualsRoot.GetNode<AnimatedSprite2D>("%Visuals/AnimatedSprite2D");

        var builder = ModAnimStateMachineBuilder.Create()
            .AddState("idle", loop: true).AsInitial().Done()
            .AddState("attack").WithNext("idle").Done()
            .AddState("hurt").WithNext("idle").Done()
            .AddState("idle2", loop: true).Done()
            .AddState("attack2").WithNext("idle2").Done()
            .AddState("hurt2").WithNext("idle2").Done()
            .AddState("special1").WithNext("idle").Done()
            .AddState("special2").WithNext("idle2").Done();

        // 有脑瓜子（形态1）播放 idle/attack/hurt，没脑瓜子（形态2）播放 idle2/attack2/hurt2
        builder.AddAnyState("Attack", "attack", () => HasBrain);
        builder.AddAnyState("Attack", "attack2", () => !HasBrain);
        builder.AddAnyState("Hit", "hurt", () => HasBrain);
        builder.AddAnyState("Hit", "hurt2", () => !HasBrain);
        builder.AddAnyState("Special1", "special1");
        builder.AddAnyState("Special2", "special2");
        // 失去/获得脑瓜子后立刻切换到对应形态的 idle 动画
        builder.AddAnyState("FormChange", "idle", () => HasBrain);
        builder.AddAnyState("FormChange", "idle2", () => !HasBrain);

        return builder.Build(new AnimatedSprite2DBackend(sprite));
    }
}
