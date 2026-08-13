using Diceomancer.Scripts.Hero.CardPool;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers;

[RegisterPower]
public class SharkTyphoonPower : ModPowerTemplate
{
    public override PowerAssetProfile AssetProfile =>
        new(
            $"res://Diceomancer/images/Power/{GetType().Name}.png",
            $"res://Diceomancer/images/Power/{GetType().Name}.png"
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card.Owner?.Creature != Owner) return;
        if (Owner.Player != null && card.VisualCardPool == Owner.Player.Character.CardPool) return;
        
        
        Flash();


        await CreatureCmd.Damage(choiceContext,
            base.CombatState.HittableEnemies, base.Amount, ValueProp.Unpowered, base.Owner);
    }
    
        
}