using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Diceomancer.Scripts.Cards.Rare;

// TODO
// [RegisterCard(typeof(DiceomancerCardPool))]
public class BorrowedCard() : ModCardTemplate(5, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
        CardKeyword.Ethereal,
        MyKeywords.Limited
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3)
    ];

    // ���ʱ��Ч���߼�?
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var selection = await CardSelectCmd.FromHand(
            prefs: new CardSelectorPrefs(base.SelectionScreenPrompt, DynamicVars.Cards.IntValue),
            context: choiceContext, player: base.Owner,
            filter: (CardModel c) => !c.IsDupe, source: this);

        var counter = selection.Count();

        for (var i = 0; i < counter; i++)
        {
            var card = selection.ElementAt(i);
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, base.Owner);
        }
    }


    // �������Ч���߼�?
    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}