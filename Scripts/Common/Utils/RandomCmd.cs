using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Diceomancer.Scripts.Common.Utils;

// 检定函数
public static class RandomCmd
{
    public static bool CheckD20(Player player, decimal amount = 0, decimal luckAmount = 0)
    {
        // 第一次掷骰
        var value = player.RunState.Rng.CombatEnergyCosts.NextInt(1, 21);
        // 取幸运骰
        for (var i = 0; i < luckAmount / 6; i++)
            value = int.Min(value, player.RunState.Rng.CombatEnergyCosts.NextInt(21));

        return value < amount;
    }

    public static int GetRandomInt(Player player, int min, int max)
    {
        return player.RunState.Rng.CombatEnergyCosts.NextInt(min, max);
    }
}