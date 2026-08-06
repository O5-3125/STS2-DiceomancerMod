using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.NormalityPower;
using Godot;
using MegaCrit.Sts2.Core.Commands;
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

// 厚皮猪猪：密林小怪
// 战斗开始时获得1层厚皮；拍肚皮与撞击两种行动模式交替，初始为拍肚皮。
[RegisterMonster]
public class Piggy : ModMonsterTemplate
{
    // 低进阶75血，高进阶80血
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 80, 75);

    public override int MaxInitialHp => MinInitialHp;

    // 拍肚皮：获得2点力量，获得6层加固
    private const int BellyStrength = 2;

    private const int BellyFortify = 6;

    // 撞击：造成7点伤害，施加1层虚弱
    private const int RamDamage = 7;

    // 怪物场景
    public override MonsterAssetProfile AssetProfile => new(
        "res://Diceomancer/scenes/Monsters/piggy/piggy.tscn"
    );

    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.VisualsScenePath!);
    }

    // 战斗开始时获得1层厚皮
    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();
        await PowerCmd.Apply<ThickSkin>(new ThrowingPlayerChoiceContext(), Creature, 1m, Creature, null);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        // 拍肚皮：获得2点力量，获得6层加固
        var patBelly = new MoveState(
            "PAT_BELLY",
            PatBellyMove,
            new BuffIntent()
        );

        // 撞击：造成7点伤害，施加1层虚弱
        var ram = new MoveState(
            "RAM",
            RamMove,
            new SingleAttackIntent(RamDamage),
            new DebuffIntent()
        );

        // 拍肚皮与撞击两种意图循环，战斗开始时是拍肚皮模式
        patBelly.FollowUpState = ram;
        ram.FollowUpState = patBelly;

        return new MonsterMoveStateMachine([patBelly, ram], patBelly);
    }

    // 拍肚皮：获得2点力量，获得6层加固
    private async Task PatBellyMove(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.TriggerAnim(Creature, "Prepare", 0.4f);
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Creature, BellyStrength, Creature, null);
        await PowerCmd.Apply<FortifiedPower>(new ThrowingPlayerChoiceContext(), Creature, BellyFortify, Creature, null);
    }

    // 撞击：造成7点伤害，施加1层虚弱
    private async Task RamMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(RamDamage)
            .FromMonster(this)
            .WithAttackerAnim("Attack", 0.4f)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);

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
            .AddState("hurt").WithNext("idle").Done()
            .AddState("prepare").WithNext("idle").Done();

        builder.AddAnyState("Attack", "attack");
        builder.AddAnyState("Hit", "hurt");
        builder.AddAnyState("Prepare", "prepare");

        return builder.Build(new AnimatedSprite2DBackend(sprite));
    }
}
