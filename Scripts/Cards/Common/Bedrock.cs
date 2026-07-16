using Diceomancer.Scripts.Capabilitys;
using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;

namespace Diceomancer.Scripts.Cards.Common;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Bedrock() : ModCardTemplate(2, CardType.Skill, CardRarity.Common, TargetType.Self, true)
{
    protected override HashSet<CardTag> CanonicalTags =>
    [
        MyTags.Evolution.GetModCardTag(),
        MyTags.Modify.GetModCardTag()
    ];

    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(12, ValueProp.Move),
        new("modify", 3),
        new DynamicVar("Evolution", 2M)
            .WithSharedTooltip("evolution")
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
        
        var cardModel = (await CardSelectCmd.FromHand(choiceContext, base.Owner,
            new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1),
            null, this)).FirstOrDefault();

        if (cardModel != null)
        {
            var capability = ModelCapabilityRegistry.Create<BlockCapability>();
            capability.DynamicVars.Block.BaseValue = DynamicVars["modify"].IntValue;
            cardModel.AddCapability(capability);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }
}