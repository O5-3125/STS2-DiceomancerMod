using Diceomancer.Scripts.Relics.Uncommon;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;

namespace Diceomancer.Scripts.Common.Patches;

// 六面骰：拾取后，之后遇到的所有遗物的数值都会被改为1-6之间的随机值
[HarmonyPatch(typeof(RelicCmd), nameof(RelicCmd.Obtain),
    new Type[] { typeof(RelicModel), typeof(Player), typeof(int) })]
public static class SixSidedDiePatch
{
    private static void Prefix(RelicModel relic, Player player)
    {
        if (relic is SixSidedDie) return;
        if (!player.Relics.Any(r => r is SixSidedDie)) return;

        foreach (DynamicVar dvar in relic.DynamicVars.Values)
        {
            if (IsSkippableVar(dvar)) continue;

            var rng = new Rng(player, relic.Id, StringHelper.GetDeterministicHashCode(dvar.Name));
            dvar.BaseValue = rng.NextInt(1, 7);
        }
    }

    private static bool IsSkippableVar(DynamicVar v)
    {
        return v.GetType().Name.Contains("Calculated", StringComparison.OrdinalIgnoreCase);
    }
}
