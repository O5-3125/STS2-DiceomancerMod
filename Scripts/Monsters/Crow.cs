using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Godot;
using STS2RitsuLib.Scaffolding.Visuals.StateMachine;
using STS2RitsuLib.Scaffolding.Visuals.StateMachine.Backends;

namespace Diceomancer.Scripts.Monsters;

// 乌鸦：密林弱怪
// 两种行为模式：攻击和干扰。
// 敌人总数小于4时意图固定为攻击模式；否则50%概率干扰模式；如果上回合是干扰模式，这回合一定是攻击模式。
// 攻击模式：先造成4点伤害，再造成1点伤害X次（X每次执行攻击意图后+1；单怪战初始2，群怪初始0；
// 场上仅有一只乌鸦时上限4，否则上限6）。
// 干扰模式：施加1层虚弱。
[RegisterMonster]
public class Crow : ModMonsterTemplate
{
    // 低进阶20血，高进阶25血
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 25, 20);

    public override int MaxInitialHp => MinInitialHp;

    // 攻击模式：先造成4点伤害，再造成1点伤害X次
    private const int BaseDamage = 4;

    private const int PeckDamage = 1;

    // X的初始值：单怪战2，群怪战0（首次使用时根据场上敌人数量惰性初始化）
    private int _peckCount = -1;

    // 上回合是否是干扰模式
    private bool _lastTurnWasInterfere;

    private int CurrentPeckCount =>
        _peckCount < 0
            ? (TotalEnemyCount <= 1 ? 2 : 0)
            : _peckCount;

    // X上限：场上仅有一只乌鸦时4，否则6
    private int MaxPeckCount => AliveCrowCount <= 1 ? 4 : 6;

    // 场上存活的乌鸦数量
    private int AliveCrowCount =>
        CombatState.Creatures.Count(c => c.Side == CombatSide.Enemy && !c.IsDead && c.Monster is Crow);

    // 敌人总数（房间内所有敌人，包括自己）
    private int TotalEnemyCount =>
        CombatState.Creatures.Count(c => c.Side == CombatSide.Enemy && !c.IsDead);

    // 怪物场景
    public override MonsterAssetProfile AssetProfile => new(
        "res://Diceomancer/scenes/Monsters/crow/crow.tscn"
    );

    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.VisualsScenePath!);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        // 攻击模式：先造成4点伤害，再造成1点伤害X次
        var attack = new MoveState(
            "ATTACK",
            AttackMove,
            new SingleAttackIntent(BaseDamage),
            new MultiAttackIntent(PeckDamage, () => CurrentPeckCount)
        );

        // 干扰模式：施加1层虚弱
        var interfere = new MoveState(
            "INTERFERE",
            InterfereMove,
            new DebuffIntent()
        );

        // 模式选择：
        // 敌人总数小于4 -> 攻击；上回合是干扰 -> 攻击；否则50%概率干扰
        var branch = new ConditionalBranchState("BRANCH");
        branch.AddState(attack, () => TotalEnemyCount < 4 || _lastTurnWasInterfere || !RollInterfere());
        branch.AddState(interfere, () => true);

        return new MonsterMoveStateMachine([branch, attack, interfere], branch);
    }

    // 50%概率选择干扰模式
    private bool RollInterfere()
    {
        return RunRng.MonsterAi.NextInt(0, 2) == 0;
    }

    // 攻击模式：先造成4点伤害，再造成1点伤害X次；执行后X+1
    private async Task AttackMove(IReadOnlyList<Creature> targets)
    {
        _lastTurnWasInterfere = false;

        await DamageCmd
            .Attack(BaseDamage)
            .FromMonster(this)
            .WithAttackerAnim("Attack", 0.4f)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);

        await DamageCmd
            .Attack(PeckDamage)
            .FromMonster(this)
            .WithHitCount(CurrentPeckCount)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);

        // 执行完攻击意图后X+1，上限按场上乌鸦数量
        _peckCount = Math.Min(CurrentPeckCount + 1, MaxPeckCount);
    }

    // 干扰模式：施加1层虚弱
    private async Task InterfereMove(IReadOnlyList<Creature> targets)
    {
        _lastTurnWasInterfere = true;

        foreach (var target in targets)
        {
            await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), target, 1m, Creature, null);
        }
    }

    protected override ModAnimStateMachine? SetupCustomCombatAnimationStateMachine(Node visualsRoot, MonsterModel monster)
    {
        var sprite = visualsRoot.GetNode<AnimatedSprite2D>("%Visuals/AnimatedSprite2D");

        var builder = ModAnimStateMachineBuilder.Create()
            .AddState("idle", loop: true).AsInitial().Done()
            .AddState("attack").WithNext("idle").Done()
            .AddState("hurt").WithNext("idle").Done();

        builder.AddAnyState("Attack", "attack");
        builder.AddAnyState("Hit", "hurt");

        return builder.Build(new AnimatedSprite2DBackend(sprite));
    }
}
