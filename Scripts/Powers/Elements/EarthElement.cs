using Diceomancer.Scripts.Cards.Token;
using Diceomancer.Scripts.Cards.Token.Essence;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.Elements;

[RegisterPower]
public class EarthElement : ElementTemplate<EssenceOfEarth>
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1)
    ];


    public override async Task Evoke(PlayerChoiceContext choiceContext)
    {
        Flash();
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner.Player);
        await PowerCmd.Decrement(this);
    }


    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;
        await Evoke(choiceContext);
    }
}