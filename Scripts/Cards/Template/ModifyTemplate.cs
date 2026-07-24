using Diceomancer.Scripts.Common;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Template;

public abstract class ModifyTemplate() : ModCardTemplate(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override HashSet<CardTag> CanonicalTags =>
    [
        MyTags.Modify.GetModCardTag()
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        new HoverTip(new LocString("static_hover_tips", "modify.title"),
            new LocString("static_hover_tips", "modify.description"))
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardModel = (await CardSelectCmd.FromHand(choiceContext,
            base.Owner, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1),
            null, this)).FirstOrDefault();

        if (cardModel != null)
            AttachCapability(cardModel);
    }

    protected abstract void AttachCapability(CardModel cardModel);

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}
