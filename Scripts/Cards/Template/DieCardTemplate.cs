using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Common.Keywords;
using Diceomancer.Scripts.Enchantments;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Template;

public abstract class DieCardTemplate<TDieEnchant>()
    : ModCardTemplate(0, CardType.Skill, CardRarity.Basic, TargetType.Self) where TDieEnchant : DieEnchant
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardModel = (await CardSelectCmd.FromHand(choiceContext, Owner,
            new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1),
            ModelDb.Enchantment<TDieEnchant>().CanEnchant, this)).FirstOrDefault();

        if (cardModel != null)
            CardCmd.Enchant<TDieEnchant>(cardModel, 1);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(MyKeywords.Bonus);
    }
}