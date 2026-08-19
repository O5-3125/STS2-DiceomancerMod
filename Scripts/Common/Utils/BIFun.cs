namespace Diceomancer.Scripts.Common.Utils;

public static class BIFun
{
    private static int BalancedIntensity(int turn)
    {
        var f = (turn - 1) % 7;

        if (f >= 2)
        {
            return 3 * (turn / 7) + f - 1;
        }
        else
        {
            return 3 * (turn / 7) + f;
        }
    }

    public static int GetCurrentVar(int baseVar, float c, int turn)
    {
        return baseVar + (int)c * BalancedIntensity(turn);
    }
}