using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Beacon() : ModCardTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
{
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        List<CardModel> cardList = base.Owner.PlayerCombatState.Hand.Cards.ToList();

        foreach (var card in cardList)
        {
            await CardCmd.Exhaust(choiceContext, card);

            (await PowerCmd.Apply<NightmarePower>(choiceContext,
                base.Owner.Creature, 1m,
                base.Owner.Creature, this)).SetSelectedCard(card);
        }
    }


    protected override void OnUpgrade()
    {
        this.AddKeyword(CardKeyword.Retain);
    }
}