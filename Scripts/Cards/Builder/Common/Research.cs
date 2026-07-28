using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Common;

[RegisterCard(typeof(BuilderCardPool))]
public class Research() : ModCardTemplate(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3),
        new("Select", 1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var pile = PileType.Draw.GetPile(Owner);
        var topCards = pile.Cards.Take(DynamicVars.Cards.IntValue).ToList();

        var selectCardModels = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner),
            Owner, new CardSelectorPrefs(SelectionScreenPrompt, 0, DynamicVars["Select"].IntValue),
            topCards.Contains)).ToList();

        if (selectCardModels.Count != 0)
        {
            foreach (var selectCardModel in selectCardModels) topCards.Remove(selectCardModel);

            await CardPileCmd.Add(selectCardModels, PileType.Hand);
        }

        await CardCmd.Discard(choiceContext, topCards);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}