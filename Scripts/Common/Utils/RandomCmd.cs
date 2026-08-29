using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Random;

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

    public static int CheckD12(Player player)
    {
        return GetRandomInt(player, 1, 12);
    }

    public static int CheckD20(Player player)
    {
        return GetRandomInt(player, 1, 20);
    }

    // 骰化：将输入值转化为多个骰子的检定
    public static int CheckDiceRoll(Player player, int n)
    {
        if (n <= 1) return CheckD4(player);

        // 2N 拆分为 20/12/8/6/4 之和：骰子数最少，且优先大骰子
        // 贪心：尽量多用大骰子，余数 == 2 时退一个（余数必为偶数，仅 2 不可由骰面凑出）
        int s = 2 * n;
        int c20 = s / 20, r = s % 20;
        if (r == 2) { c20--; r = 22; }

        int c12 = r / 12; r %= 12;
        if (r == 2) { c12--; r = 14; }

        int c8 = r / 8; r %= 8;
        if (r == 2) { c8--; r = 10; }

        int c6 = r / 6; r %= 6;
        if (r == 2) { c6--; r = 8; }

        int c4 = r / 4;

        int sum = 0;
        for (int i = 0; i < c20; i++) sum += CheckD20(player);
        for (int i = 0; i < c12; i++) sum += CheckD12(player);
        for (int i = 0; i < c8;  i++) sum += CheckD8(player);
        for (int i = 0; i < c6;  i++) sum += CheckD6(player);
        for (int i = 0; i < c4;  i++) sum += CheckD4(player);
        return sum;
    }

    public static int GetRandomInt(Player player, int min, int max)
    {
        // NextInt 的 max 是排他上限，D4 需要 1-4，所以这里加1
        return player.RunState.Rng.CombatEnergyCosts.NextInt(min, max + 1);
    }
}