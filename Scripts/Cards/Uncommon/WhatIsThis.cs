using Diceomancer.Scripts.Common;
using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Exceptions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
// [RegisterCharacterStarterCard(typeof(DiceomancerCharacter))]
public class WhatIsThis() : ModCardTemplate(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
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
        List<CardPoolModel> cardPoolList = base.Owner.UnlockState.CharacterCardPools.ToList();

        IEnumerable<CardModel> cardList = from c in cardPoolList.SelectMany(c =>
                c.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
            )
            // where c.Rarity != CardRarity.Rare
            select c;

        var list = CardFactory.GetDistinctForCombat(base.Owner,
            cardList,
            Math.Min(cardList.Count(), this.DynamicVars.Cards.IntValue),
            base.Owner.RunState.Rng.CombatCardGeneration).ToList();

        foreach (var item2 in await CardSelectCmd.FromSimpleGrid(choiceContext, list, base.Owner,
                     new CardSelectorPrefs(this.SelectionScreenPrompt, 0, this.DynamicVars.Cards.IntValue)))
            await CardPileCmd.AddGeneratedCardToCombat(item2, PileType.Hand, base.Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Evolution"].UpgradeValueBy(1);
    }
}