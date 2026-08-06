using Diceomancer.Scripts.Powers.MonstersPower;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Godot;
using STS2RitsuLib.Scaffolding.Visuals.StateMachine;
using STS2RitsuLib.Scaffolding.Visuals.StateMachine.Backends;

namespace Diceomancer.Scripts.Monsters;

// 神选草：密林精英
// 战斗开始时获得6层覆甲；格挡被击破时对玩家造成12点伤害。
// 单一意图：每回合造成10点伤害。
[RegisterMonster]
public class GrassGuy : ModMonsterTemplate
{
    // 低进阶150血，高进阶180血
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 72, 75);

    public override int MaxInitialHp => MinInitialHp;

    // 战斗开始时的覆甲层数
    private const int InitialPlating = 6;

    // 每回合造成10点伤害
    private const int BasicDamage = 10;

    // 怪物场景
    public override MonsterAssetProfile AssetProfile => new(
        "res://Diceomancer/scenes/Monsters/GrassGuy/GrassGuy.tscn"
    );

    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.VisualsScenePath!);
    }

    // 战斗开始时获得6层覆甲和荆棘反击被动
    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();
        await PowerCmd.Apply<PlatingPower>(new ThrowingPlayerChoiceContext(), Creature, InitialPlating, Creature, null);
        await PowerCmd.Apply<GrassRetaliationPower>(new ThrowingPlayerChoiceContext(), Creature, 1m, Creature, null);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        // 单一意图：每回合造成10点伤害
        var basicAttack = new MoveState(
            "BASIC_ATTACK",
            BasicAttackMove,
            new SingleAttackIntent(BasicDamage)
        );

        basicAttack.FollowUpState = basicAttack;
        return new MonsterMoveStateMachine([basicAttack], basicAttack);
    }

    private async Task BasicAttackMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(BasicDamage)
            .FromMonster(this)
            .WithAttackerAnim("Attack", 0.4f)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
    }

    protected override ModAnimStateMachine? SetupCustomCombatAnimationStateMachine(Node visualsRoot, MonsterModel monster)
    {
        var sprite = visualsRoot.GetNode<AnimatedSprite2D>("%Visuals/AnimatedSprite2D");

        var builder = ModAnimStateMachineBuilder.Create()
            .AddState("idle", loop: true).AsInitial().Done()
            .AddState("attack").WithNext("idle").Done()
            .AddState("hurt").WithNext("idle").Done()
            .AddState("block").WithNext("idle").Done();

        builder.AddAnyState("Attack", "attack");
        builder.AddAnyState("Hit", "hurt");
        builder.AddAnyState("Block", "block");

        return builder.Build(new AnimatedSprite2DBackend(sprite));
    }
}
