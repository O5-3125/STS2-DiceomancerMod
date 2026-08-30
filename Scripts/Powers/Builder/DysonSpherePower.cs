using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers;

[RegisterPower]
public class DysonSpherePower : ModPowerTemplate
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

        Flash();
        await PlayerCmd.GainEnergy(Amount, card.Owner);
    }
}