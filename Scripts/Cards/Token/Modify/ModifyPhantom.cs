using Diceomancer.Scripts.Capabilitys;
using Diceomancer.Scripts.Common;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Token.Modify;

[RegisterCard(typeof(TokenCardPool))]
public class ModifyPhantom()
    : ModCardTemplate(1, CardType.Skill, CardRarity.Token, TargetType.Self)
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
        new DynamicVar("modify", 1)
            .WithSharedTooltip("modify")
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardModel = (await CardSelectCmd.FromHand(choiceContext,
            base.Owner,
            new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1),
            null,
            this)).FirstOrDefault();

        var capability = ModelCapabilityRegistry.Create<PhantomCapability>();
        capability.DynamicVars.Cards.BaseValue = DynamicVars["modify"].IntValue;
        cardModel?.AddCapability(capability);

        // cardModel?.GetOrCreateCapability<PhantomCapability>(); // 挂载组件
    }


    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}