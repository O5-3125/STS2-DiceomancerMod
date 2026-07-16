using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Diceomancer.Scripts.Common.Utils;

// 检定函数
public static class RandomCmd
{
    public static int CheckD4(Player player)
    {
        return GetRandomInt(player, 1, 4);
    }

    public static int CheckD6(Player player)
    {
        return GetRandomInt(player, 1, 6);
    }

    public static int CheckD8(Player player)
    {
        return GetRandomInt(player, 1, 8);
    }

    public static int CheckD10(Player player)
    {
        return GetRandomInt(player, 1, 10);
    }

    public static int CheckD12(Player player)
    {
        return GetRandomInt(player, 1, 12);
    }

    public static int CheckD20(Player player)
    {
        return GetRandomInt(player, 1, 20);
    }

    public static int GetRandomInt(Player player, int min, int max)
    {
        return player.RunState.Rng.CombatEnergyCosts.NextInt(min, max);
    }
}