using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.Berserker;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Diceomancer.Scripts.Common.Utils;

public static class BerserkerCardUtils
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

    public static async Task ConvertFrenzyToRage(PlayerChoiceContext choiceContext, Player player, AbstractModel source)
    {
        var creature = player.Creature;
        var frenzy = creature.GetPower<FrenzyPower>();
        if (frenzy is null || frenzy.Amount <= 0) return;

        var currentRage = SecondaryResourceStateStore.GetAmount(player, Rage.Id);
        var maxRage = SecondaryResourceStateStore.GetMaxAmount(player, Rage.Id) ?? currentRage;
        if (currentRage >= maxRage) return;

        var gain = Math.Min((int)frenzy.Amount, maxRage - currentRage);
        if (gain <= 0) return;

        await PowerCmd.ModifyAmount(choiceContext, frenzy, -gain, creature, null, true);
        await SecondaryResourceCmd.Gain(player, Rage.Id, gain, source);
    }
}