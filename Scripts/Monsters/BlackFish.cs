using Diceomancer.Scripts.Powers.MonstersPower;
using Diceomancer.Scripts.Powers.NormalityPower;
using Godot;
using MegaCrit.Sts2.Core.Commands;
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

// 黑色的鱼：荣耀层boss
// 诅咒被动：玩家除第一回合外每回合开始选择诅咒。
// 三个行动模式循环：重击 -> 连击 -> buff，初始为重击。
[RegisterMonster]
public class BlackFish : ModMonsterTemplate
{
    // 1000血
    public override int MinInitialHp => 1000;

    public override int MaxInitialHp => 1000;

    // 重击：造成20伤害
    private const int HeavyDamage = 20;

    // 连击：造成5点伤害5次
    private const int ComboDamage = 5;

    private const int ComboCount = 5;

    // buff：获得20层加固，获得2层力量
    private const int FortifyAmount = 20;

    private const int StrengthAmount = 2;

    // 怪物场景
    public override MonsterAssetProfile AssetProfile => new(
        "res://Diceomancer/scenes/Monsters/blackfish/blackfish.tscn"
    );

    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.VisualsScenePath!);
    }

    // 战斗开始时，获得诅咒被动
    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();

        await PowerCmd.Apply<BlackfishCursePower>(new ThrowingPlayerChoiceContext(),
            Creature, 1m, Creature, null);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        // 重击：造成20伤害
        var heavyStrike = new MoveState(
            "HEAVY_STRIKE",
            HeavyStrikeMove,
            new SingleAttackIntent(HeavyDamage)
        );

        // 连击：造成5点伤害5次
        var combo = new MoveState(
            "COMBO",
            ComboMove,
            new MultiAttackIntent(ComboDamage, ComboCount)
        );

        // buff：获得20层加固，获得2层力量
        var buff = new MoveState(
            "BUFF",
            BuffMove,
            new BuffIntent()
        );

        // 初始始终是重击，然后按顺序循环
        heavyStrike.FollowUpState = combo;
        combo.FollowUpState = buff;
        buff.FollowUpState = heavyStrike;

        return new MonsterMoveStateMachine([heavyStrike, combo, buff], heavyStrike);
    }

    // 重击：造成20伤害
    private async Task HeavyStrikeMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(HeavyDamage)
            .FromMonster(this)
            .WithAttackerAnim("Attack", 0.4f)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
    }

    // 连击：造成5点伤害5次
    private async Task ComboMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(ComboDamage)
            .FromMonster(this)
            .WithHitCount(ComboCount)
            .WithAttackerAnim("Attack2", 0.4f)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
    }

    // buff：获得20层加固，获得2层力量
    private async Task BuffMove(IReadOnlyList<Creature> targets)
    {
        await CreatureCmd.TriggerAnim(Creature, "Shield", 0.4f);
        await PowerCmd.Apply<FortifiedPower>(new ThrowingPlayerChoiceContext(), Creature, FortifyAmount, Creature,
            null);
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Creature, StrengthAmount, Creature,
            null);
    }

    protected override ModAnimStateMachine? SetupCustomCombatAnimationStateMachine(Node visualsRoot,
        MonsterModel monster)
    {
        var sprite = visualsRoot.GetNode<AnimatedSprite2D>("%Visuals/AnimatedSprite2D");

        var builder = ModAnimStateMachineBuilder.Create()
            .AddState("idle", loop: true).AsInitial().Done()
            .AddState("attack").WithNext("idle").Done()
            .AddState("attack2").WithNext("idle").Done()
            .AddState("hurt").WithNext("idle").Done()
            .AddState("shield").WithNext("idle").Done();

        builder.AddAnyState("Attack", "attack");
        builder.AddAnyState("Attack2", "attack2");
        builder.AddAnyState("Hit", "hurt");
        builder.AddAnyState("Shield", "shield");

        return builder.Build(new AnimatedSprite2DBackend(sprite));
    }
}