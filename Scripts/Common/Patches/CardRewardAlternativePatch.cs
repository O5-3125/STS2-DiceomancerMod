using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;

namespace Diceomancer.Scripts.Common.Patches;

// 原版 CardRewardAlternative.Generate 只支持至多2个备选选项，超过会抛异常。
// 移除该异常，使骰子遗物与 PaelsWing（献祭）、梦枕（+2生命）等原版添加选项的遗物共存。
[HarmonyPatch(typeof(CardRewardAlternative), nameof(CardRewardAlternative.Generate))]
public static class CardRewardAlternativePatch
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var codes = instructions.ToList();
        for (var i = 0; i < codes.Count; i++)
        {
            if (codes[i].opcode != OpCodes.Throw) continue;

            // 仅当紧邻的 ldstr + newobj InvalidOperationException 模式匹配时才移除，
            // 防止因编译产物差异破坏栈平衡。
            if (i >= 2
                && codes[i - 1].opcode == OpCodes.Newobj
                && codes[i - 2].opcode == OpCodes.Ldstr
                && codes[i - 1].operand is ConstructorInfo { DeclaringType: { } type } ctor
                && type == typeof(InvalidOperationException)
                && ctor.GetParameters().Length == 1
                && ctor.GetParameters()[0].ParameterType == typeof(string))
            {
                codes.RemoveRange(i - 2, 3);
            }
            break;
        }
        return codes;
    }
}