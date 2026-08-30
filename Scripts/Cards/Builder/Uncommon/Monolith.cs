using Diceomancer.Scripts.Capabilitys;
using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;

namespace Diceomancer.Scripts.Cards.Builder.Uncommon;

[RegisterCard(typeof(BuilderCardPool))]
public class Monolith() : EvolutionTemplate(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, 2M)
{
    protected override IEnumerable<CardTag> OwnCanonicalTags =>
    [
        MyTags.Modify.GetModCardTag()
    ];

    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
        new BlockVar(12, ValueProp.Move),
        new("modify", 3)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

        var cardModel = (await CardSelectCmd.FromHand(choiceContext, Owner,
            new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1),
            null, this)).FirstOrDefault();

        if (cardModel != null)
        {
            var capability = ModelCapabilityRegistry.Create<BlockCapability>();
            capability.DynamicVars["BlockCapability"].BaseValue = DynamicVars["modify"].IntValue;
            cardModel.AddCapability(capability);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
    }
}