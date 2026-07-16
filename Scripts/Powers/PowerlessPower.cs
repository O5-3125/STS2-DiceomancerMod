using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers;

[RegisterPower]
public class PowerlessPower :ModPowerTemplate  //TemporaryStrengthPower 
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override PowerAssetProfile AssetProfile => new(
        "res://Diceomancer/images/Power/无力.png",
        "res://Diceomancer/images/Power/无力.png"
    );
    
    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource, CardPlay? cardPlay)
    {
        if (dealer != Owner && dealer != null && !Owner.Pets.Contains(dealer)) return 0m;
    
        if (!props.IsPoweredAttack()) return 0m;
    
        // if (cardSource == null)
        // {
        //     return 0m;
        // }
        
        return -Amount;
        
    }
    
    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer,
        DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer != null && (dealer == Owner || dealer.PetOwner?.Creature == Owner) && props.IsPoweredAttack())
        {
            await PowerCmd.Remove(this);
        }
    }
    
    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Side) return;
    
        await PowerCmd.Remove(this);
    }
}