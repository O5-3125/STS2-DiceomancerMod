using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Diceomancer.Scripts.Common.Utils;

public static class DiceomancerCardCmd
{
    private static readonly NormalityBuffKind[] NormalityBuffKinds =
    [
        NormalityBuffKind.Strength, // 力量
        NormalityBuffKind.Dexterity, // 敏捷
        NormalityBuffKind.Focus, // 集中
        NormalityBuffKind.Buffer, // 缓冲
        NormalityBuffKind.Intangible, // 无实体
        NormalityBuffKind.Plating, // 覆甲
        NormalityBuffKind.Regen, // 再生
        NormalityBuffKind.RetainHand, //  保留
        NormalityBuffKind.Vigor, // 活力
        NormalityBuffKind.Thorns, // 荆棘
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
            case NormalityBuffKind.Intangible:
                await PowerCmd.Apply<IntangiblePower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.Plating:
                await PowerCmd.Apply<PlatingPower>(choiceContext, target, amount, applier, cardSource);
                break;
            case NormalityBuffKind.Regen:
                await PowerCmd.Apply<RagePower>(choiceContext, target, amount, applier, cardSource);
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
            default:
                await PowerCmd.Apply<BufferPower>(choiceContext, target, amount, applier, cardSource);
                break;
        }
    }

    // // // //
    private static readonly NormalityDebuffKind[] NormalityDebuffKinds =
    [
        NormalityDebuffKind.Poison, // 毒
        NormalityDebuffKind.Doom, // 灾厄
        NormalityDebuffKind.Demise, // 消亡
        NormalityDebuffKind.Frail, // 脆弱
        NormalityDebuffKind.Vulnerable, // 易伤
        NormalityDebuffKind.Weak, // 虚弱
    ];

    public static async Task ApplyRandomDebuff(PlayerChoiceContext choiceContext, Player owner, Creature target,
        Creature applier, CardModel? cardSource, decimal amount)
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
        Creature target, Creature applier, CardModel? cardSource, decimal amount)
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
            default:
                await PowerCmd.Apply<DemisePower>(choiceContext, target, amount, applier, cardSource);
                break;
        }
    }
}