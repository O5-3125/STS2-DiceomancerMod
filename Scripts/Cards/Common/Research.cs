using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Common;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Research() : ModCardTemplate(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3),
        new DynamicVar("Select", 1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var pile = PileType.Draw.GetPile(Owner);
        var topCards = pile.Cards.Take(DynamicVars.Cards.IntValue).ToList();

        var selectCardModels = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(base.Owner),
            base.Owner, new CardSelectorPrefs(base.SelectionScreenPrompt, 0, DynamicVars["Select"].IntValue),
            topCards.Contains)).ToList();

        if (selectCardModels.Count != 0)
        {
            foreach (var selectCardModel in selectCardModels)
            {
                topCards.Remove(selectCardModel);
            }

            await CardPileCmd.Add(selectCardModels, PileType.Hand);
        }
        await CardCmd.Discard(choiceContext, topCards);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}