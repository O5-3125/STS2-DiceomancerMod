using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.CardPool;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Design()
    : ModCardTemplate(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = CardFactory.GetDistinctForCombat(base.Owner,
            ModelDb.CardPool<DiceomancerCardPool>().AllCards
                .Where(card => card.Tags.Contains(MyTags.Upgrade.GetModCardTag())),
            DynamicVars.Cards.IntValue,
            base.Owner.RunState.Rng.CombatCardGeneration).ToList();
        
        if (base.IsUpgraded)
        {
            CardCmd.Upgrade(cards, CardPreviewStyle.HorizontalLayout);
        }

        var cardModel =
            await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, base.Owner, canSkip: true);
        if (cardModel != null)
        {
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, base.Owner);
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}