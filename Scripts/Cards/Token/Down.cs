using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Common.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Token;

[RegisterCard(typeof(TokenCardPool))]
public class Down() : ModCardTemplate(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, MyKeywords.Chaos4];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1),
        new("Down", 1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardModels =
            (await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(SelectionScreenPrompt, 0,
                    DynamicVars.Cards.IntValue),
                context: choiceContext, player: Owner, filter: null, source: this)).ToArray();


        ModifyCardCmd.ModifyCardListDynamicVarsAdditive(cardModels, (int)-DynamicVars["Down"].BaseValue);


    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}