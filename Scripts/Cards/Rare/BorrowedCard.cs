using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Rare;

[RegisterCard(typeof(ColorlessCardPool))]
public class BorrowedCard() : ModCardTemplate(0, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

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

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var selection = (await CardSelectCmd.FromHand(choiceContext,
            base.Owner,
            new CardSelectorPrefs(base.SelectionScreenPrompt, 0, DynamicVars.Cards.IntValue),
            // (CardModel c) => c.VisualCardPool.IsColorless,
            null,
            this)).ToList();

        foreach (var card in selection.Select(cardModel => cardModel.CreateClone()))
        {
            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, base.Owner);
        }
    }

    protected override void OnUpgrade()
    {
        this.RemoveKeyword(CardKeyword.Exhaust);
    }
}