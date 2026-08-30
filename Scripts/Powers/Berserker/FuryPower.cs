using Diceomancer.Scripts.Relics.Starter;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.Berserker;

[RegisterPower]
public class FuryPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/{GetType().Name}.png",
        $"res://Diceomancer/images/Power/{GetType().Name}.png"
    );

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner.Player is null) return;
        if (Amount <= 0) return;
        Flash();
        var relic = Owner.Player.GetRelic<EmberCore>();
        if (relic == null)
        {
            await PlayerCmd.GainEnergy(Amount, Owner.Player);
            await PowerCmd.Remove(this);
        }
        else
        {
            for (var i = 0; i < relic.DynamicVars["MaxEnergyCap"].IntValue; i++)
            {
                await PlayerCmd.GainEnergy(1, Owner.Player);
                await PowerCmd.Decrement(this);
            }
        }
    }
}