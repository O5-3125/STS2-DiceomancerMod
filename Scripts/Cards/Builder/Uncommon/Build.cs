using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Uncommon;

[RegisterCard(typeof(BuilderCardPool))]
public class Build() : ModCardTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(4, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

        var upgradeCards = PileType.Hand.GetPile(Owner).Cards
            .Where(c => c.Tags.Contains(MyTags.Upgrade.GetModCardTag()))
            .ToList();

        if (upgradeCards.Count == 0) return;

        var selected = (await CardSelectCmd.FromHand(choiceContext, Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, 0, 1),
            c => c.Tags.Contains(MyTags.Upgrade.GetModCardTag()), this)).FirstOrDefault();

        if (selected == null) return;

        var targetType = selected.GetType();
        while (targetType != null)
        {
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(UpgradeTemplate<>))
                break;
            targetType = targetType.BaseType;
        }

        if (targetType == null) return;

        var transformType = targetType.GetGenericArguments()[0];
        var template = ModelDb.AllCards.FirstOrDefault(c => c.GetType() == transformType);

        if (template == null) return;
        var newCard = CombatState.CreateCard(template, Owner);
        // var newCard = Owner.RunState.CreateCard(template, Owner);
        if (selected.IsUpgraded) CardCmd.Upgrade(newCard);
        await CardCmd.Transform(selected, newCard);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2);
    }
}