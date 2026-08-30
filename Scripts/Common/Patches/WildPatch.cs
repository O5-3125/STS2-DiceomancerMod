using Diceomancer.Scripts.Common.Keywords;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Diceomancer.Scripts.Common.Patches;

// 狂野：能量不足时也可以打出。若是打出时能量不足不会扣除能量，改为每点能量需求对玩家造成2点伤害
[HarmonyPatch(typeof(PlayerCombatState), nameof(PlayerCombatState.HasEnoughResourcesFor))]
public static class WildPlayablePatch
{
    private static bool Prefix(CardModel card, out UnplayableReason reason, ref bool __result,
        PlayerCombatState __instance)
    {
        if (!Wild.HasWild(card))
        {
            reason = UnplayableReason.None;
            return true;
        }

        // 狂野牌忽略能量需求，只检查星辰费用
        int starCost = Math.Max(0, card.GetStarCostWithModifiers());
        if (starCost > __instance.Stars)
        {
            reason = UnplayableReason.StarCostTooHigh;
            __result = false;
        }
        else
        {
            reason = UnplayableReason.None;
            __result = true;
        }
        return false;
    }
}

[HarmonyPatch(typeof(CardModel), "SpendEnergy")]
public static class WildSpendEnergyPatch
{
    private static void Prefix(CardModel __instance, ref int amount)
    {
        if (amount <= 0 || !Wild.HasWild(__instance)) return;

        var playerCombatState = __instance.Owner.PlayerCombatState;
        if (playerCombatState == null || amount <= playerCombatState.Energy) return;

        // 能量不足：不扣除能量，每点能量需求对玩家造成2点伤害
        int selfDamage = amount * 2;
        amount = 0;

        var creature = __instance.Owner.Creature;
        TaskHelper.RunSafely(CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), creature, selfDamage,
            ValueProp.Unpowered | ValueProp.Move, __instance, null));
    }
}

// 狂野：能量不足时也可以打出，此时牌发出红光提示
[HarmonyPatch(typeof(CardModel), nameof(CardModel.ShouldGlowRed), MethodType.Getter)]
public static class WildGlowRedPatch
{
    private static void Postfix(CardModel __instance, ref bool __result)
    {
        if (__result || !__instance.IsMutable || !Wild.HasWild(__instance)) return;

        var playerCombatState = __instance.Owner.PlayerCombatState;
        if (playerCombatState == null) return;

        int cost = __instance.EnergyCost.GetAmountToSpend();
        if (cost > playerCombatState.Energy) __result = true;
    }
}
