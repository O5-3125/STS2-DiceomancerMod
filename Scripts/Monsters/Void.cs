using Diceomancer.Scripts.Powers.MonstersPower;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
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

// 空图层：荣耀boss
// 被动能力：数据错误（玩家每回合打出10张牌，手牌数字全部-1，下限0）。
// 意图：第1回合仅召唤海鸥；从第2回合起造成6点伤害4次，且每回合尝试召唤一只海鸥（最多5只）。
[RegisterMonster]
public class Void : ModMonsterTemplate
{
    // 1000血
    public override int MinInitialHp => 1;

    public override int MaxInitialHp => 1;

    // 攻击：造成6点伤害4次
    private const int AttackDamage = 6;

    private const int AttackCount = 4;

    // 场上最多5只海鸥
    private const int MaxSeagulls = 0;

    // 怪物场景
    public override MonsterAssetProfile AssetProfile => new(
        "res://Diceomancer/scenes/Monsters/void/void.tscn"
    );

    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.VisualsScenePath!);
    }

    // 战斗开始时，获得数据错误被动
    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();
        
        await PowerCmd.Apply<VoidDataErrorPower>(new ThrowingPlayerChoiceContext(), 
            Creature, 1m, Creature, null);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        // 第1回合：召唤海鸥
        var summon = new MoveState(
            "SUMMON",
            SummonMove,
            new SummonIntent()
        );

        // 第2回合起：造成6点伤害4次 + 每回合召唤海鸥
        var attack = new MoveState(
            "ATTACK",
            AttackMove,
            new MultiAttackIntent(AttackDamage, AttackCount)
        );

        summon.FollowUpState = attack;
        attack.FollowUpState = attack;

        return new MonsterMoveStateMachine([summon, attack], summon);
    }

    // 第1回合：仅召唤海鸥
    private async Task SummonMove(IReadOnlyList<Creature> targets)
    {
        await SummonSeagull();
    }

    // 第2回合起：召唤海鸥，然后造成6点伤害4次
    private async Task AttackMove(IReadOnlyList<Creature> targets)
    {
        await SummonSeagull();

        await DamageCmd
            .Attack(AttackDamage)
            .FromMonster(this)
            .WithHitCount(AttackCount)
            .WithAttackerAnim("Attack", 0.4f)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
    }

    // 每回合尝试召唤一只海鸥（场上最多5只）
    private async Task SummonSeagull()
    {
        var seagullCount = CombatState.Creatures.Count(c => !c.IsDead && c.Monster is SeaGull);
        if (seagullCount >= MaxSeagulls) return;

        await CreatureCmd.TriggerAnim(Creature, "Special1", 0.6f);
        Creature target = await CreatureCmd.Add(ModelDb.Monster<SeaGull>().ToMutable(),
            base.CombatState, CombatSide.Enemy, base.CombatState.Encounter.GetNextSlot(base.CombatState));

        // await CreatureCmd.Add<SeaGull>(CombatState);
    }

    protected override ModAnimStateMachine? SetupCustomCombatAnimationStateMachine(Node visualsRoot,
        MonsterModel monster)
    {
        var sprite = visualsRoot.GetNode<AnimatedSprite2D>("%Visuals/AnimatedSprite2D");

        var builder = ModAnimStateMachineBuilder.Create()
            .AddState("idle", loop: true).AsInitial().Done()
            .AddState("attack").WithNext("idle").Done()
            .AddState("hurt").WithNext("idle").Done()
            .AddState("special1").WithNext("idle").Done();

        builder.AddAnyState("Attack", "attack");
        builder.AddAnyState("Hit", "hurt");
        builder.AddAnyState("Special1", "special1");

        return builder.Build(new AnimatedSprite2DBackend(sprite));
    }
}