using Diceomancer.Scripts.Hero.Barbarian;
using Diceomancer.Scripts.Orbs;
using Diceomancer.Scripts.Orbs.Elements;
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
        return creatures.Aggregate(0m, (current, creature) => current + creature.GetPowerAmount<BurnPower>());
    }

    public static int CountElementOrbs(Player player)
    {
        var orbs = player.PlayerCombatState?.OrbQueue.Orbs.ToList();

        return orbs?.Count(model => model is ElementOrbTemplate) ?? 0;
    }

    public static async Task EvokeElementOrb(PlayerChoiceContext choiceContext, Player player, int count)
    {
        for (var i = 0; i < count; i++)
        {
            await OrbCmd.EvokeNext(choiceContext, player);
        }
    }
}