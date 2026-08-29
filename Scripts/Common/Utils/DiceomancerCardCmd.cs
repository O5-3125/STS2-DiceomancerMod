using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Powers.Mocks;

namespace Diceomancer.Scripts.Common.Utils;

public static class DiceomancerCardCmd
{
    // 增益
    private delegate Task ApplyBuff(PlayerChoiceContext choiceContext, Creature target, decimal amount,
        Creature applier, CardModel? cardSource);

    // 单一数据源：新增/删除一个增益只需改这一处
    private static readonly (NormalityBuffKind Kind, ApplyBuff Apply)[] NormalityBuffs =
    [
        (NormalityBuffKind.Strength, (c, t, a, ap, s) => PowerCmd.Apply<StrengthPower>(c, t, a, ap, s)), // 力量
        (NormalityBuffKind.Dexterity, (c, t, a, ap, s) => PowerCmd.Apply<DexterityPower>(c, t, a, ap, s)), // 敏捷
        (NormalityBuffKind.Focus, (c, t, a, ap, s) => PowerCmd.Apply<FocusPower>(c, t, a, ap, s)), // 集中
        (NormalityBuffKind.Plating, (c, t, a, ap, s) => PowerCmd.Apply<PlatingPower>(c, t, a, ap, s)), // 覆甲
        (NormalityBuffKind.Regen, (c, t, a, ap, s) => PowerCmd.Apply<RegenPower>(c, t, a, ap, s)), // 再生
        (NormalityBuffKind.RetainHand, (c, t, a, ap, s) => PowerCmd.Apply<RetainHandPower>(c, t, a, ap, s)), // 保留
        (NormalityBuffKind.Vigor, (c, t, a, ap, s) => PowerCmd.Apply<VigorPower>(c, t, a, ap, s)), // 活力
        (NormalityBuffKind.Thorns, (c, t, a, ap, s) => PowerCmd.Apply<ThornsPower>(c, t, a, ap, s)), // 荆棘
        (NormalityBuffKind.Haste, (c, t, a, ap, s) => PowerCmd.Apply<HastePower>(c, t, a, ap, s)), // 加速
        (NormalityBuffKind.Evade, (c, t, a, ap, s) => PowerCmd.Apply<EvadePower>(c, t, a, ap, s)), // 闪避
        (NormalityBuffKind.CriticalHit, (c, t, a, ap, s) => PowerCmd.Apply<CriticalHit>(c, t, a, ap, s)), // 暴击
        (NormalityBuffKind.BlockNextTurn, (c, t, a, ap, s) => PowerCmd.Apply<BlockNextTurnPower>(c, t, a, ap, s)), //下回合格挡
        (NormalityBuffKind.Fortified, (c, t, a, ap, s) => PowerCmd.Apply<FortifiedPower>(c, t, a, ap, s)), // 加固
        (NormalityBuffKind.Toughness, (c, t, a, ap, s) => PowerCmd.Apply<ToughnessPower>(c, t, a, ap, s)), // 坚韧
    ];

    public static async Task ApplyRandomBuff(PlayerChoiceContext choiceContext, Player owner, Creature target,
        Creature applier, CardModel? cardSource, decimal amount)
    {
        var entry = owner.RunState.Rng.CombatCardSelection.NextItem(NormalityBuffs);
        await entry.Apply(choiceContext, target, amount, applier, cardSource);
    }


// 减益
    private delegate Task ApplyDebuff(PlayerChoiceContext choiceContext, Creature target, decimal amount,
        Creature? applier, CardModel? cardSource);

    private static readonly (NormalityDebuffKind Kind, ApplyDebuff Apply)[] NormalityDebuffs =
    [
        (NormalityDebuffKind.Poison, (c, t, a, ap, s) => PowerCmd.Apply<PoisonPower>(c, t, a, ap, s)), // 毒
        (NormalityDebuffKind.Doom, (c, t, a, ap, s) => PowerCmd.Apply<DoomPower>(c, t, a, ap, s)), // 灾厄
        (NormalityDebuffKind.Demise, (c, t, a, ap, s) => PowerCmd.Apply<DemisePower>(c, t, a, ap, s)), // 消亡
        (NormalityDebuffKind.Frail, (c, t, a, ap, s) => PowerCmd.Apply<FrailPower>(c, t, a, ap, s)), // 脆弱
        (NormalityDebuffKind.Vulnerable, (c, t, a, ap, s) => PowerCmd.Apply<VulnerablePower>(c, t, a, ap, s)), // 易伤
        (NormalityDebuffKind.Weak, (c, t, a, ap, s) => PowerCmd.Apply<WeakPower>(c, t, a, ap, s)), // 虚弱
        (NormalityDebuffKind.Bleed, (c, t, a, ap, s) => PowerCmd.Apply<BleedPower>(c, t, a, ap, s)), // 流血
        (NormalityDebuffKind.Burn, (c, t, a, ap, s) => PowerCmd.Apply<BurnPower>(c, t, a, ap, s)), // 燃烧
        // (NormalityDebuffKind.Blind, (c, t, a, ap, s) => PowerCmd.Apply<BlindPower>(c, t, a, ap, s)), // 目盲
        (NormalityDebuffKind.Powerless, (c, t, a, ap, s) => PowerCmd.Apply<PowerlessPower>(c, t, a, ap, s)), // 无力
        (NormalityDebuffKind.ThinSkin, (c, t, a, ap, s) => PowerCmd.Apply<ThinSkinPower>(c, t, a, ap, s)), // 脆皮
    ];

    public static async Task ApplyRandomDebuff(PlayerChoiceContext choiceContext, Player owner, Creature target,
        Creature? applier, CardModel? cardSource, decimal amount)
    {
        var entry = owner.RunState.Rng.CombatCardSelection.NextItem(NormalityDebuffs);
        await entry.Apply(choiceContext, target, amount, applier, cardSource);
    }

    public static async Task ApplyAllDebuff(PlayerChoiceContext choiceContext, Creature target, Creature applier,
        CardModel? cardSource, decimal amount)
    {
        foreach (var entry in NormalityDebuffs)
            await entry.Apply(choiceContext, target, amount, applier, cardSource);
    }
}