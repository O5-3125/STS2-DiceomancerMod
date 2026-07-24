using Diceomancer.Scripts.Cards.Token;
using Diceomancer.Scripts.Common;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers;

[RegisterPower]
public class FireworkArrayPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;


    public override PowerAssetProfile AssetProfile => new(
        "res://Diceomancer/images/Power/累坏了.png",
        "res://Diceomancer/images/Power/累坏了.png"
    );


    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        for (var i = 0; i < Amount; i++)
        {
            CardModel cardModel = combatState.CreateCard<FireworkRocket>(player);

            CardCmd.PreviewCardPileAdd(
                await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, player, CardPilePosition.Random),
                2.2f);
        }
    }
}