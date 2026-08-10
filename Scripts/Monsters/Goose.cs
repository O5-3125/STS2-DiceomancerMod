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

// 鹅：密林小怪
// 战斗开始时获得特殊能力捣蛋鬼；每回合随机获得一种弃牌buff并攻击。
// 伤害每3回合递增：1-3回合8点，4-6回合12点，7-9回合16点...
[RegisterMonster]
public class Goose : ModMonsterTemplate
{
    // 低进阶35血，高进阶40血
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 40, 35);

    public override int MaxInitialHp => MinInitialHp;

    // 基础攻击8点，每3回合+4点
    private int AttackDamage => 8 + 4 * ((CombatState.RoundNumber - 1) / 3);

    // 怪物场景
    public override MonsterAssetProfile AssetProfile => new(
        "res://Diceomancer/scenes/Monsters/goose/goose.tscn"
    );

    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.VisualsScenePath!);
    }

    // 战斗开始时获得特殊能力捣蛋鬼
    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();
        await PowerCmd.Apply<GooseTroublemakerPower>(new ThrowingPlayerChoiceContext(), Creature, 1m, Creature, null);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        // 每回合：随机获得一种弃牌buff + 攻击意图
        var attack = new MoveState(
            "PECK_ATTACK",
            AttackMove,
            new SingleAttackIntent(AttackDamage)
        );

        attack.FollowUpState = attack;
        return new MonsterMoveStateMachine([attack], attack);
    }

    // 攻击：先随机获得一种弃牌buff，再造成伤害
    private async Task AttackMove(IReadOnlyList<Creature> targets)
    {


        await DamageCmd
            .Attack(AttackDamage)
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
            .AddState("sa").WithNext("idle").Done();

        builder.AddAnyState("Attack", "attack");
        builder.AddAnyState("Hit", "hurt");
        // 获得弃牌buff时的特殊动画
        builder.AddAnyState("Discard", "sa");

        return builder.Build(new AnimatedSprite2DBackend(sprite));
    }
}
