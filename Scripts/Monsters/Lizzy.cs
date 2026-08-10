using Diceomancer.Scripts.Powers.MonstersPower;
using Diceomancer.Scripts.Powers.NormalityPower;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models.Powers;
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


[RegisterMonster]
public class Lizzy : ModMonsterTemplate
{
    // 300血
    public override int MinInitialHp => 300;

    public override int MaxInitialHp => 300;

    // 大嘴巴子扇你：造成12伤害，施加1层易伤
    private const int SlapDamage = 12;

    // 大嗓门使劲吼：造成4伤害3次，获得2点力量，施加3层负担
    private const int YellDamage = 4;

    private const int YellCount = 3;

    private const int YellStrength = 2;

    private const int YellBurden = 3;

    // 怪物场景
    public override MonsterAssetProfile AssetProfile => new(
        "res://Diceomancer/scenes/Monsters/lizzy/lizzy.tscn"
    );

    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.VisualsScenePath!);
    }

    // 战斗开始时获得标记能力，负责每回合检查弃牌buff
    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();
        // await PowerCmd.Apply<LizzyBuffPower>(new ThrowingPlayerChoiceContext(), Creature, 1m, Creature, null);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        // 大嘴巴子扇你：造成12伤害，施加1层易伤
        var slap = new MoveState(
            "SLAP",
            SlapMove,
            new SingleAttackIntent(SlapDamage),
            new DebuffIntent()
        );

        // 大嗓门使劲吼：造成4伤害3次，获得2点力量，施加3层负担
        var yell = new MoveState(
            "YELL",
            YellMove,
            new MultiAttackIntent(YellDamage, YellCount),
            new BuffIntent(),
            new DebuffIntent()
        );

        // 两种模式交替，初始为大嘴巴子扇你
        slap.FollowUpState = yell;
        yell.FollowUpState = slap;

        return new MonsterMoveStateMachine([slap, yell], slap);
    }

    // 大嘴巴子扇你：造成12伤害，施加1层易伤
    private async Task SlapMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(SlapDamage)
            .FromMonster(this)
            .WithAttackerAnim("Attack", 0.4f)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);

        foreach (var target in targets)
        {
            await PowerCmd.Apply<VulnerablePower>(new ThrowingPlayerChoiceContext(), target, 1m, Creature, null);
            

        }
    }

    // 大嗓门使劲吼：造成4伤害3次，获得2点力量，施加3层负担
    private async Task YellMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(YellDamage)
            .FromMonster(this)
            .WithHitCount(YellCount)
            .WithAttackerAnim("Special", 0.4f)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);

        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Creature, YellStrength, Creature, null);

        foreach (var target in targets)
        {
            await PowerCmd.Apply<BurdenPower>(new ThrowingPlayerChoiceContext(), target, YellBurden, Creature, null);
        }
    }

    protected override ModAnimStateMachine? SetupCustomCombatAnimationStateMachine(Node visualsRoot, MonsterModel monster)
    {
        var sprite = visualsRoot.GetNode<AnimatedSprite2D>("%Visuals/AnimatedSprite2D");

        var builder = ModAnimStateMachineBuilder.Create()
            .AddState("idle", loop: true).AsInitial().Done()
            .AddState("attack").WithNext("idle").Done()
            .AddState("hurt").WithNext("idle").Done()
            .AddState("special").WithNext("idle").Done();

        builder.AddAnyState("Attack", "attack");
        builder.AddAnyState("Hit", "hurt");
        builder.AddAnyState("Special", "special");

        return builder.Build(new AnimatedSprite2DBackend(sprite));
    }
}
