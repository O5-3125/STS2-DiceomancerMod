using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Uncommon;

[RegisterCard(typeof(BuilderCardPool))]
public class ChaoticTransform() : ModCardTemplate(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3),
        new("selectCount", 1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var modifyCardModels = CardFactory.GetDistinctForCombat(Owner,
            ModelDb.CardPool<TokenCardPool>().AllCards
                .Where(model => model.Tags.Contains(MyTags.Modify.GetModCardTag())),
            DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardGeneration).ToList();

        // var cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, base.Owner, canSkip: true);

        var cardModels = await CardSelectCmd.FromSimpleGrid
        (choiceContext, modifyCardModels, Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, 0, DynamicVars["selectCount"].IntValue));

        foreach (var cardModel in cardModels)
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner);
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        AddKeyword(MyKeywords.Phantom);
    }
}