using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Monsters;

// [RegisterMonster]
public class Crow : ModMonsterTemplate
{
    // 根据进阶提高最小血量
    public override int MinInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 25, 20);

    // 根据进阶提高最大血量
    public override int MaxInitialHp => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 25, 20);

    // 意图1的数值
    private int BasicDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 1, 1);
    private int BasicRepeat => Math.Min(CombatState.RoundNumber + 1, 4);

    // 意图2的数值，重击伤害，根据进阶提高伤害
    private int HeavyDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 6);

    // 怪物场景
    public override MonsterAssetProfile AssetProfile => new(
        "res://Test/scenes/test_monster.tscn"
    );

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        // 意图1：造成伤害，获得格挡
        var basicAttack = new MoveState(
            "BASIC_ATTACK", // 状态ID
            BasicAttackMove,

            // 以下是可变参数，可以填写任意数量的意图，全部展示
            new SingleAttackIntent(HeavyDamage),
            new MultiAttackIntent(BasicDamage, BasicRepeat)
        );

        basicAttack.FollowUpState = basicAttack;
        return new MonsterMoveStateMachine([basicAttack], basicAttack);
    }


    private async Task BasicAttackMove(IReadOnlyList<Creature> targets)
    {
        // 重击
        await DamageCmd
            .Attack(HeavyDamage)
            .FromMonster(this)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
        // 连击
        await DamageCmd
            .Attack(BasicDamage)
            .FromMonster(this)
            .WithHitCount(BasicRepeat)
            .WithAttackerFx(null, AttackSfx)
            .WithHitFx("vfx/vfx_attack_blunt")
            .Execute(null);
    }
}