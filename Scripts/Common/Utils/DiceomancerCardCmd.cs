using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Powers.Mocks;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace Diceomancer.Scripts.Common.Utils;

public static class DiceomancerCardCmd
{
    // 增益池
    private static readonly NormalityBuffKind[] NormalityBuffKinds =
    [
        NormalityBuffKind.Strength, // 力量
        NormalityBuffKind.Dexterity, // 敏捷
        NormalityBuffKind.Focus, // 集中
        NormalityBuffKind.Plating, // 覆甲
        NormalityBuffKind.Regen, // 再生
        NormalityBuffKind.RetainHand, //  保留
        NormalityBuffKind.Vigor, // 活力
        NormalityBuffKind.Thorns, // 荆棘
        NormalityBuffKind.Haste, // 加速
        NormalityBuffKind.Evade, // 闪避 免疫下次伤害
        NormalityBuffKind.CriticalHit, // 暴击
        NormalityBuffKind.BlockNextTurn,
        NormalityBuffKind.Fortified, // 加固
        NormalityBuffKind.Toughness, // 坚韧
    ];

    public static async Task ApplyRandomBuff(PlayerChoiceContext choiceContext, Player owner, Creature target,
        Creature applier, CardModel? cardSource, decimal amount)
    {
        var kind = PickRandomBuff(owner, target);
        await ApplyRandomBuff(choiceContext, kind, target, applier, cardSource, amount);
    }

    private static NormalityBuffKind PickRandomBuff(Player owner, Creature target)
    {
        var combatCardSelection = owner.RunState.Rng.CombatCardSelection;
        return new NormalityBuffKind?(combatCardSelection.NextItem(NormalityBuffKinds)).GetValueOrDefault();
    }

    private static async Task ApplyRandomBuff(PlayerChoiceContext choiceContext, NormalityBuffKind kind,
        Creature target, Creature applier, CardModel? cardSource, decimal amount)
    {
        switch (kind)
        {
            case NormalityBuffKind.Strength:
                await PowerCmd.Apply<StrengthPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.Dexterity:
                await PowerCmd.Apply<DexterityPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.Focus:
                await PowerCmd.Apply<FocusPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.Buffer:
                await PowerCmd.Apply<BufferPower>(choiceContext, target, amount, applier, cardSource);
                break;

            case NormalityBuffKind.Plating:
                await PowerCmd.Apply<PlatingPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.Regen:
                await PowerCmd.Apply<RegenPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.RetainHand:
                await PowerCmd.Apply<RetainHandPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.Vigor:
                await PowerCmd.Apply<VigorPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.Thorns:
                await PowerCmd.Apply<ThornsPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.Haste:
                await PowerCmd.Apply<HastePower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.Evade:
                await PowerCmd.Apply<EvadePower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.CriticalHit:
                await PowerCmd.Apply<CriticalHit>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.Fortified:
                await PowerCmd.Apply<FortifiedPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.Toughness:
                await PowerCmd.Apply<ToughnessPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.BlockNextTurn:
                await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.EnergyNextTurn:
                await PowerCmd.Apply<EnergyNextTurnPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.DrawCardsNextTurn:
                await PowerCmd.Apply<DrawCardsNextTurnPower>(choiceContext, target, amount, applier, cardSource);
                break;
            default:
                // await PowerCmd.Apply<BufferPower>(choiceContext, target, amount, applier, cardSource);
                break;
        }
    }

    // 减益池
    private static readonly NormalityDebuffKind[] NormalityDebuffKinds =
    [
        NormalityDebuffKind.Poison, // 毒
        NormalityDebuffKind.Doom, // 灾厄
        NormalityDebuffKind.Demise, // 消亡
        NormalityDebuffKind.Frail, // 脆弱
        NormalityDebuffKind.Vulnerable, // 易伤
        NormalityDebuffKind.Weak, // 虚弱
        NormalityDebuffKind.Bleed, // 流血
        NormalityDebuffKind.Burn, // 燃烧
        // NormalityDebuffKind.Blind, // 目盲  本回合下次攻击伤害为0
        NormalityDebuffKind.Strength, // 无力 力量-1
        NormalityDebuffKind.Powerless, // 无力 
        NormalityDebuffKind.Tainted // 脆皮/污染
    ];

    public static async Task ApplyRandomDebuff(PlayerChoiceContext choiceContext, Player owner, Creature target,
        Creature? applier, CardModel? cardSource, decimal amount)
    {
        var kind = PickRandomDebuff(owner, target);
        await ApplyRandomDebuff(choiceContext, kind, target, applier, cardSource, amount);
    }

    private static NormalityDebuffKind PickRandomDebuff(Player owner, Creature target)
    {
        var combatCardSelection = owner.RunState.Rng.CombatCardSelection;

        return new NormalityDebuffKind?(combatCardSelection.NextItem(NormalityDebuffKinds)).GetValueOrDefault();
    }

    private static async Task ApplyRandomDebuff(PlayerChoiceContext choiceContext, NormalityDebuffKind kind,
        Creature target, Creature? applier, CardModel? cardSource, decimal amount)
    {
        switch (kind)
        {
            case NormalityDebuffKind.Poison:
                await PowerCmd.Apply<PoisonPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityDebuffKind.Doom:
                await PowerCmd.Apply<DoomPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityDebuffKind.Demise:
                await PowerCmd.Apply<DemisePower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityDebuffKind.Frail:
                await PowerCmd.Apply<FrailPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityDebuffKind.Vulnerable:
                await PowerCmd.Apply<VulnerablePower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityDebuffKind.Weak:
                await PowerCmd.Apply<WeakPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityDebuffKind.Bleed:
                await PowerCmd.Apply<BleedPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityDebuffKind.Burn:
                await PowerCmd.Apply<BurnPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityDebuffKind.Strength:
                await PowerCmd.Apply<StrengthPower>(choiceContext, target, -amount, applier, cardSource);
                break;
            case NormalityDebuffKind.Tainted:
                await PowerCmd.Apply<WeakPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityDebuffKind.Powerless:
                await PowerCmd.Apply<PowerlessPower>(choiceContext, target, amount, applier, cardSource);
                break;
            default:
                // await PowerCmd.Apply<DemisePower>(choiceContext, target, amount, applier, cardSource);
                break;
        }
    }


    public static async Task ApplyAllDebuff(PlayerChoiceContext choiceContext, Creature target, Creature applier,
        CardModel? cardSource, decimal amount)
    {
        await PowerCmd.Apply<PoisonPower>(choiceContext, target, amount, applier, cardSource);
        await PowerCmd.Apply<DoomPower>(choiceContext, target, amount, applier, cardSource);
        await PowerCmd.Apply<DemisePower>(choiceContext, target, amount, applier, cardSource);
        await PowerCmd.Apply<FrailPower>(choiceContext, target, amount, applier, cardSource);
        await PowerCmd.Apply<VulnerablePower>(choiceContext, target, amount, applier, cardSource);
        await PowerCmd.Apply<WeakPower>(choiceContext, target, amount, applier, cardSource);
        await PowerCmd.Apply<DemisePower>(choiceContext, target, amount, applier, cardSource);
    }
}