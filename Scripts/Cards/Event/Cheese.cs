using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Event;

[RegisterCard(typeof(ColorlessCardPool))]
public class Cheese() : ModCardTemplate(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HealVar(99m),
        new CardsVar(2)
    ];


    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        // CardKeyword.Unplayable,
        MyKeywords.Fragile
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(base.Owner.Creature, base.DynamicVars.Heal.BaseValue);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);


        // if (cardPlay.Card != this || cardPlay.Card.Owner != base.Owner)
        // return;
        
        if (PileType.Deck.GetPile(Owner).Cards.Contains(cardPlay.Card.DeckVersion))
            await CardPileCmd.RemoveFromDeck(cardPlay.Card.DeckVersion);

        await CardPileCmd.RemoveFromCombat(cardPlay.Card);
        
    }
}