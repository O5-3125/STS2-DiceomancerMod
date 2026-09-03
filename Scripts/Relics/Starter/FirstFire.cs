using Diceomancer.Scripts.Hero.Barbarian;
using Diceomancer.Scripts.Powers.Berserker;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Starter;

[RegisterRelic(typeof(BarbarianRelicPool))]
[RegisterCharacterStarterRelic(typeof(Barbarian))]
public class FirstFire : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Ancient;


    public override RelicAssetProfile AssetProfile => new(
        IconPath: $"res://Diceomancer/images/Relics/{GetType().Name}.png",
        IconOutlinePath: $"res://Diceomancer/images/Relics/{GetType().Name}.png",
        BigIconPath: $"res://Diceomancer/images/Relics/{GetType().Name}.png"
    );


    public override bool ShowCounter => true;

    public override int DisplayAmount => DynamicVars["MaxEnergyCap"].IntValue;

    public void UpdateDisplayAmount()
    {
        AssertMutable();
        // _cardsPlayed = value;
        InvokeDisplayAmountChanged();
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new("MaxEnergyCap", 5)
    ];

    public override decimal ModifyEnergyGain(Player player, decimal amount)
    {
        if (player != Owner) return amount;

        return Math.Max(0m, Math.Min(amount, DynamicVars["MaxEnergyCap"].IntValue - player.PlayerCombatState.Energy));
    }

    public override async Task AfterEnergyReset(Player player)
    {
        if (player != Owner) return;


        if (player.GetEnergy() > DynamicVars["MaxEnergyCap"].IntValue)
        {
            await PlayerCmd.SetEnergy(DynamicVars["MaxEnergyCap"].IntValue, Owner);
        }
    }


    public override Task AfterCombatEnd(CombatRoom room)
    {
        DynamicVars["MaxEnergyCap"].BaseValue = 5;


        return base.AfterCombatEnd(room);
    }

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(base.Owner.Creature)) return;

        await PowerCmd.Apply<FuryPower>(choiceContext, Owner.Creature, base.Owner.PlayerCombatState.Energy, null, null);
    }
}