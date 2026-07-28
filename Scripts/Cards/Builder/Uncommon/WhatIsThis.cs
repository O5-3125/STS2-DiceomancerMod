using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Uncommon;

[RegisterCard(typeof(BuilderCardPool))]
// [RegisterCharacterStarterCard(typeof(DiceomancerCharacter))]
public class WhatIsThis() : ModCardTemplate(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    // public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override HashSet<CardTag> CanonicalTags =>
    [
        MyTags.Evolution.GetModCardTag()
    ];


    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3),
        new DynamicVar("Evolution", 2M)
            .WithSharedTooltip("evolution")
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardPoolList = Owner.UnlockState.CharacterCardPools.ToList();

        IEnumerable<CardModel> cardList = from c in cardPoolList.SelectMany(c =>
                c.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
            )
            // where c.Rarity != CardRarity.Rare
            select c;

        var list = CardFactory.GetDistinctForCombat(Owner,
            cardList,
            Math.Min(cardList.Count(), DynamicVars.Cards.IntValue),
            Owner.RunState.Rng.CombatCardGeneration).ToList();

        foreach (var item2 in await CardSelectCmd.FromSimpleGrid(choiceContext, list, Owner,
                     new CardSelectorPrefs(SelectionScreenPrompt, 0, DynamicVars.Cards.IntValue)))
            await CardPileCmd.AddGeneratedCardToCombat(item2, PileType.Hand, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Evolution"].UpgradeValueBy(1);
    }
}