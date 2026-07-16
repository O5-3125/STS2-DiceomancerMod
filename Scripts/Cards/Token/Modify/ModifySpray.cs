using Diceomancer.Scripts.Capabilitys;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero.CardPool;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.PotionPools;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Token.Modify;

[RegisterCard(typeof(TokenCardPool))]
public class ModifySpray()
    : ModCardTemplate(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override HashSet<CardTag> CanonicalTags =>
    [
        MyTags.Modify.GetModCardTag()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        // new DamageVar(3, ValueProp.Move)
        new DynamicVar("modify", 1)
            .WithSharedTooltip("modify")
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardModel = (await CardSelectCmd.FromHand(choiceContext, base.Owner,
            new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1),
            null, this)).FirstOrDefault();
        var capability = ModelCapabilityRegistry.Create<SprayCapability>();
        capability.DynamicVars.Damage.BaseValue = DynamicVars["modify"].IntValue;
        cardModel?.AddCapability(capability);
        // cardModel?.GetOrCreateCapability<SprayCapability>(); // 挂载组件
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}