using MegaCrit.Sts2.Core.Entities.Creatures;

namespace Diceomancer.Scripts.Common.Utils;

// 检定函数
public static class Checks
{
    public static bool CheckD20(Creature? dealer, decimal amount = 0, decimal luckAmount = 0)
    {
        // 第一次掷骰
        var value = dealer.Player.RunState.Rng.CombatEnergyCosts.NextInt(1, 21);
        // 取幸运骰
        for (var i = 0; i < luckAmount / 6; i++)
            value = int.Min(value, dealer.Player.RunState.Rng.CombatEnergyCosts.NextInt(21));

        return value < amount;
    }
}