using Diceomancer.Scripts.Powers.MonstersPower;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
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

// 火猫三丈：荣耀怪物
// 被动：玩家回合开始时手牌数字随机变化（掷骰子）。
// 每回合意图固定：造成4点伤害2次。
[RegisterMonster]
public class Cat : ModMonsterTemplate
{
    // 低进阶210血，高进阶220血
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 220, 210);

    public override int MaxInitialHp => MinInitialHp;

    // 每回合造成4点伤害2次
    private const int AttackDamage = 4;

    private const int AttackCount = 2;

    // 怪物场景
    public override MonsterAssetProfile AssetProfile => new(
        "res://Diceomancer/scenes/Monsters/cat/cat.tscn"
    );

    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.VisualsScenePath!);
    }

    // 战斗开始时，给所有玩家施加手牌数字变化被动
    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();

        await PowerCmd.Apply<CatNumberChaosPower>(new ThrowingPlayerChoiceContext(),
            Creature, 1m, Creature, null);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        // 每回合意图固定：造成4点伤害2次
        var basicAttack = new MoveState(
            "BASIC_ATTACK",
            BasicAttackMove,
            new MultiAttackIntent(AttackDamage, AttackCount)
        );

        basicAttack.FollowUpState = basicAttack;
        return new MonsterMoveStateMachine([basicAttack], basicAttack);
    }

    private async Task BasicAttackMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(AttackDamage)
            .FromMonster(this)
            .WithHitCount(AttackCount)
            .WithAttackerAnim("Attack", 0.4f)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
    }

    protected override ModAnimStateMachine? SetupCustomCombatAnimationStateMachine(Node visualsRoot,
        MonsterModel monster)
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