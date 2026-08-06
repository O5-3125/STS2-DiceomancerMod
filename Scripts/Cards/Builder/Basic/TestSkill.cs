using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Basic;

[RegisterCard(typeof(TokenCardPool))]
public class TestSkill()
    : ModCardTemplate(0, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1),
        new("Down", 1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardModels =
            (await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 0,
                    DynamicVars.Cards.IntValue),
                context: choiceContext, player: Owner, filter: null, source: this)).ToArray();

        foreach (var cardModel in cardModels)
        {
            CardCmd.Enchant<Adroit>(cardModel, 1);
        }
    }
}