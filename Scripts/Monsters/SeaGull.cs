using Diceomancer.Scripts.Cards.Token;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Godot;
using STS2RitsuLib.Scaffolding.Visuals.StateMachine;
using STS2RitsuLib.Scaffolding.Visuals.StateMachine.Backends;

namespace Diceomancer.Scripts.Monsters;

// 海鸥：空图层召唤的小怪
// 五种行动模式循环：攻击、移除减益、塞薯条、加数字、给增益。
// 被召唤时等概率选择初始行动模式，然后按顺序循环；每个行动模式产生恰好一个意图。
[RegisterMonster]
public class SeaGull : ModMonsterTemplate
{
    // 海鸥50血
    public override int MinInitialHp => 50;

    public override int MaxInitialHp => 50;

    // 攻击：造成3伤害3次
    private const int AttackDamage = 3;

    private const int AttackCount = 3;

    // 塞薯条：加3张薯条到抽牌堆
    private const int FriesCount = 3;

    // 加数字：最大生命值+2，所有可叠加能力层数+2
    private const int NumberBonus = 2;

    // 给增益：所有友方获得2层力量
    private const int StrengthBonus = 2;

    // 怪物场景
    public override MonsterAssetProfile AssetProfile => new(
        "res://Diceomancer/scenes/Monsters/SeaGull/SeaGull.tscn"
    );

    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.VisualsScenePath!);
    }

    protected override MonsterMoveStateMachine GenerateMoveStateMachine()
    {
        // 攻击：造成3伤害3次
        var attack = new MoveState(
            "ATTACK",
            AttackMove,
            new MultiAttackIntent(AttackDamage, AttackCount)
        );

        // 移除减益：移除所有友方的减益
        var cleanse = new MoveState(
            "CLEANSE",
            CleanseMove,
            new BuffIntent()
        );

        // 塞薯条：加3张薯条到你的抽牌堆
        var stuffFries = new MoveState(
            "STUFF_FRIES",
            StuffFriesMove,
            new StatusIntent(FriesCount)
        );

        // 加数字：所有友方的生命值和能力层数加2
        var addNumbers = new MoveState(
            "ADD_NUMBERS",
            AddNumbersMove,
            new BuffIntent()
        );

        // 给增益：所有友方获得2层力量
        var giveBuffs = new MoveState(
            "GIVE_BUFFS",
            GiveBuffsMove,
            new BuffIntent()
        );

        // 五种行动模式按顺序循环
        attack.FollowUpState = cleanse;
        cleanse.FollowUpState = stuffFries;
        stuffFries.FollowUpState = addNumbers;
        addNumbers.FollowUpState = giveBuffs;
        giveBuffs.FollowUpState = attack;

        // 被召唤时等概率选择一个初始行动模式
        var initial = new RandomBranchState("INITIAL");
        initial.AddBranch(attack, MoveRepeatType.CannotRepeat, () => 0.2f);
        initial.AddBranch(cleanse, MoveRepeatType.CannotRepeat, () => 0.2f);
        initial.AddBranch(stuffFries, MoveRepeatType.CannotRepeat, () => 0.2f);
        initial.AddBranch(addNumbers, MoveRepeatType.CannotRepeat, () => 0.2f);
        initial.AddBranch(giveBuffs, MoveRepeatType.CannotRepeat, () => 0.2f);

        return new MonsterMoveStateMachine(
            [initial, attack, cleanse, stuffFries, addNumbers, giveBuffs],
            initial);
    }

    // 攻击：造成3伤害3次
    private async Task AttackMove(IReadOnlyList<Creature> targets)
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

    // 移除减益：移除所有友方（不包括自己）的减益
    private async Task CleanseMove(IReadOnlyList<Creature> targets)
    {
        
        foreach (var ally in GetAllies())
        {
            foreach (var power in ally.Powers.Where(p => p.Type == PowerType.Debuff).ToList())
            {
                await PowerCmd.Remove(power);
            }
        }
    }

    // 塞薯条：加3张薯条到玩家的抽牌堆
    private async Task StuffFriesMove(IReadOnlyList<Creature> targets)
    {
        var player = CombatState.RunState.Players.FirstOrDefault();
        if (player == null) return;

        for (var i = 0; i < FriesCount; i++)
        {
            var fries = CombatState.CreateCard<Fries>(player);
            await CardPileCmd.Add(fries, PileType.Draw);
        }
    }

    // 加数字：所有友方（不包括自己）的最大生命值+2，所有可叠加能力层数+2
    private async Task AddNumbersMove(IReadOnlyList<Creature> targets)
    {
        foreach (var ally in GetAllies())
        {
            await CreatureCmd.GainMaxHp(ally, NumberBonus);

            foreach (var power in ally.Powers.Where(p => p.StackType == PowerStackType.Counter).ToList())
            {
                await PowerCmd.ModifyAmount(new ThrowingPlayerChoiceContext(), power, NumberBonus, null, null);
            }
        }
    }

    // 给增益：所有友方（不包括自己）获得2层力量
    private async Task GiveBuffsMove(IReadOnlyList<Creature> targets)
    {
        foreach (var ally in GetAllies())
        {
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), ally, StrengthBonus, Creature, null);
        }
    }

    // 友方范围：同阵营、不包括自己、未死亡
    private IEnumerable<Creature> GetAllies()
    {
        return CombatState.Creatures.Where(c => !c.IsDead && c.Side == Creature.Side && c != Creature);
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
