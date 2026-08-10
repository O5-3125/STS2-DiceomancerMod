using Diceomancer.Scripts.Common;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Token.Modify;

[RegisterCard(typeof(TokenCardPool))]
public class Hindsight() : ModCardTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override HashSet<CardTag> CanonicalTags =>
    [
        MyTags.Modify.GetModCardTag()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1)
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardModels =
            (await CardSelectCmd.FromCombatPile(
                prefs: new CardSelectorPrefs(SelectionScreenPrompt, 0, DynamicVars.Cards.IntValue),
                context: choiceContext, pile: PileType.Discard.GetPile(Owner), player: Owner)).ToList();


        foreach (var cardModel in cardModels.Where(cardModel => cardModel.Keywords.Contains(CardKeyword.Exhaust)))
            cardModel.RemoveKeyword(CardKeyword.Exhaust);

        await CardPileCmd.Add(cardModels, PileType.Draw, CardPilePosition.Random);
    }

    protected override void OnUpgrade()
    {
        // RemoveKeyword(CardKeyword.Exhaust);
        EnergyCost.UpgradeBy(-1);
    }
}