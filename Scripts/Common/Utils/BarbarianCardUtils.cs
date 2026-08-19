using Diceomancer.Scripts.Hero.Barbarian;
using Diceomancer.Scripts.Orbs;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.Berserker;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Diceomancer.Scripts.Common.Utils;

public static class BarbarianCardUtils
{
    public static async Task HealInjury(PlayerChoiceContext choiceContext, Creature creature, decimal amount)
    {
        if (amount < 1m) return;
        var injury = creature.GetPower<Injury>();
        if (injury == null) return;
        await PowerCmd.ModifyAmount(choiceContext, injury, -Math.Min(amount, injury.Amount), null, null);
    }

    public static decimal TotalBurnCount(IEnumerable<Creature> creatures)
    {
        var total = 0m;
        foreach (var creature in creatures)
        {
            total += creature.GetPowerAmount<BurnPower>();
        }
        return total;
    }

    public static int CountEmotionOrbs(Player player)
    {
        return player.PlayerCombatState?.OrbQueue.Orbs.Count ?? 0;
    }

    public static async Task ConvertFuryToOrbs(PlayerChoiceContext choiceContext, Player player)
    {
        var frenzy = player.Creature.GetPower<FuryPower>();
        if (frenzy is null || frenzy.Amount <= 0) return;

        var capacity = player.PlayerCombatState?.OrbQueue.Capacity ?? 0;
        var count = player.PlayerCombatState?.OrbQueue.Orbs.Count ?? 0;
        var empty = capacity - count;
        if (empty <= 0) return;

        var gain = Math.Min((int)frenzy.Amount, empty);
        if (gain <= 0) return;

        await PowerCmd.ModifyAmount(choiceContext, frenzy, -gain, player.Creature, null, true);
        await ChannelEmotionOrbs(choiceContext, player, gain);
    }

    public static async Task ChannelEmotionOrbs(PlayerChoiceContext choiceContext, Player player, int count)
    {
        for (var i = 0; i < count; i++)
        {
            await OrbCmd.Channel<EmotionOrb>(choiceContext, player);
        }
    }

    public static async Task EvokeEmotionOrbs(PlayerChoiceContext choiceContext, Player player, int count)
    {
        for (var i = 0; i < count; i++)
        {
            await OrbCmd.EvokeNext(choiceContext, player);
        }
    }
}