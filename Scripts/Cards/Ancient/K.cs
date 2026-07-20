using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Ancient;

// 加入哪个卡池
[RegisterCard(typeof(DiceomancerCardPool))]
public class K()
    : ModCardTemplate(1, CardType.Skill, CardRarity.Ancient, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1),
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // var cardPool = ModelDb.AllCards;
        // var cardPool = base.Owner.Character.CardPool.AllCards.ToList();

        var cardPool = base.Owner.RunState.Rng.CombatCardGeneration.NextItem(ModelDb.AllCharacterCardPools)?.AllCards
            .ToList();

        if (cardPool == null)
        {
            return;
        }

        var cardModels = CardFactory.GetDistinctForCombat(base.Owner, cardPool, cardPool.Count,
            base.Owner.RunState.Rng.CombatCardGeneration).ToList();

        var cards = await CardSelectCmd.FromSimpleGrid(choiceContext, cardModels, base.Owner,
            new CardSelectorPrefs(this.SelectionScreenPrompt, 0, DynamicVars.Cards.IntValue));


        foreach (CardModel card in cards)
        {
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, base.Owner);
        }
    }


    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png" // 卡图
    );


    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}