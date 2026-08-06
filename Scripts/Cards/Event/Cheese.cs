using Diceomancer.Scripts.Common;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Event;

// [RegisterCard(typeof(EventCardPool))]
public class Cheese() : ModCardTemplate(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

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
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);


        // if (cardPlay.Card != this || cardPlay.Card.Owner != base.Owner)
        // return;

        if (PileType.Deck.GetPile(Owner).Cards.Contains(cardPlay.Card.DeckVersion))
            await CardPileCmd.RemoveFromDeck(cardPlay.Card.DeckVersion);

        await CardPileCmd.RemoveFromCombat(cardPlay.Card);
    }
}