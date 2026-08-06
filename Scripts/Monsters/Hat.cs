using Diceomancer.Scripts.Cards.Token;
using Diceomancer.Scripts.Powers.MonstersPower;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Godot;
using STS2RitsuLib.Scaffolding.Visuals.StateMachine;
using STS2RitsuLib.Scaffolding.Visuals.StateMachine.Backends;

namespace Diceomancer.Scripts.Monsters;

// 大红帽子怪：虫巢2层怪物
// 被动：如果每回合打出的第一张牌目标不是他，获得3层闪避。
// 两种行动模式交替：丢炸弹 -> 准备炸弹，初始为丢炸弹。
[RegisterMonster]
public class Hat : ModMonsterTemplate
{
    // 低进阶70血，高进阶80血
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 120, 100);

    public override int MaxInitialHp => MinInitialHp;

    // 丢炸弹：获得丢炸弹buff，造成4伤害
    private const int BombDamage = 4;

    private const int BombCard = 2;


    // 准备炸弹：造成6点伤害2次
    private const int PrepareDamage = 6;

    private const int PrepareCount = 2;

    // 怪物场景
    public override MonsterAssetProfile AssetProfile => new(
        "res://Diceomancer/scenes/Monsters/hat/hat.tscn"
    );

    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.VisualsScenePath!);
    }

    // 战斗开始时获得被动能力
    public override async Task AfterAddedToRoom()
    {
        await base.AfterAddedToRoom();
        await PowerCmd.Apply<HatPassivePower>(new ThrowingPlayerChoiceContext(), Creature, 2m, Creature, null);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        // 丢炸弹：丢2张炸弹，造成4伤害
        var throwBomb = new MoveState(
            "THROW_BOMB",
            ThrowBombMove,
            new SingleAttackIntent(BombDamage), new StatusIntent(BombCard)
        );

        // 准备炸弹：造成6点伤害2次
        var prepareBomb = new MoveState(
            "PREPARE_BOMB",
            PrepareBombMove,
            new MultiAttackIntent(PrepareDamage, PrepareCount), new BuffIntent()
        );

        // 两种模式交替，初始为丢炸弹
        throwBomb.FollowUpState = prepareBomb;
        prepareBomb.FollowUpState = throwBomb;

        return new MonsterMoveStateMachine([throwBomb, prepareBomb], throwBomb);
    }

    // 丢炸弹：丢炸弹，造成4伤害
    private async Task ThrowBombMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(BombDamage)
            .FromMonster(this)
            .WithAttackerAnim("Attack", 0.4f)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);

        await CardPileCmd.AddToCombatAndPreview<HotBomb>(targets, PileType.Draw, 2, null);
    }

    // 准备炸弹：造成6点伤害2次
    private async Task PrepareBombMove(IReadOnlyList<Creature> targets)
    {
        await DamageCmd
            .Attack(PrepareDamage)
            .FromMonster(this)
            .WithHitCount(PrepareCount)
            .WithAttackerAnim("Attack", 0.4f)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);

        await PowerCmd.Apply<HatPassivePower>(new ThrowingPlayerChoiceContext(), Creature, 2m, Creature, null);
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