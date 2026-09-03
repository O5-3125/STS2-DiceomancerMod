using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Colorless;

[RegisterCard(typeof(ColorlessCardPool))]
public class WhatIsThis() : ModCardTemplate(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1),
        new CardsVar(3),
        new CardsVar("Select", 1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (IsUpgraded)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        }

        var cardPoolList = Owner.UnlockState.CharacterCardPools.ToList();

        List<CardModel> cardList = (from c in cardPoolList.SelectMany(c =>
                c.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
            )
            // where c.Rarity != CardRarity.Rare
            select c).ToList();

        var list = CardFactory.GetDistinctForCombat(Owner,
            cardList,
            DynamicVars.Cards.IntValue,
            Owner.RunState.Rng.CombatCardGeneration).ToList();

        foreach (var item in await CardSelectCmd.FromSimpleGrid(choiceContext, list, Owner,
                     new CardSelectorPrefs(SelectionScreenPrompt, 0, DynamicVars["Select"].IntValue)))
            await CardPileCmd.AddGeneratedCardToCombat(item, PileType.Hand, Owner);
    }

    protected override void OnUpgrade()
    {
    }
}