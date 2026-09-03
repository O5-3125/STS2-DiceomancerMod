using Diceomancer.Scripts.Cards.Barbarian.Basic;
using Diceomancer.Scripts.Hero.Barbarian;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.Berserker;
using Diceomancer.Scripts.Powers.NormalityPower;
using Diceomancer.Scripts.Relics.Starter;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Diceomancer.Scripts.Common.Utils;

public static class BarbarianCmd
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

    public static async Task GainMaxEnergyCap(Player player, int cap)
    {
        var relic = player.GetRelic<FirstFire>();
        if (relic == null) return;

        relic.DynamicVars["MaxEnergyCap"].BaseValue += cap;

        UpdateDisplayAmount(relic);
    }

    public static async Task LossMaxEnergyCap(Player player, int cap)
    {
        var relic = player.GetRelic<FirstFire>();
        if (relic == null) return;

        relic.DynamicVars["MaxEnergyCap"].BaseValue -= cap;


        UpdateDisplayAmount(relic);
    }

    public static async Task SetMaxEnergyCap(Player player, int newEnergyCap)
    {
        var relic = player.GetRelic<FirstFire>();
        if (relic == null) return;

        relic.DynamicVars["MaxEnergyCap"].BaseValue = newEnergyCap;

        UpdateDisplayAmount(relic);
    }

    private static void UpdateDisplayAmount(FirstFire firstFire)
    {
        firstFire.Flash();
        firstFire.UpdateDisplayAmount();
    }
}