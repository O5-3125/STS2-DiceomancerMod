using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.MonstersPower;


[RegisterPower]
public class HatPassivePower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/MonstersPower/{GetType().Name}.png",
        $"res://Diceomancer/images/Power/MonstersPower/{GetType().Name}.png"
    );
    
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target != Owner)
        {
            Flash();
            await PowerCmd.Apply<EvadePower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
        }
        else
        {
            await PowerCmd.Remove(this);
        }
    }
}