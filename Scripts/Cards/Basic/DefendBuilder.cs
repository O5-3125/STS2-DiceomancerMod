using Diceomancer.Scripts.Common;
using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interactions.RightClick;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;

namespace Diceomancer.Scripts.Cards.Basic;

[RegisterCard(typeof(DiceomancerCardPool))]
[RegisterCharacterStarterCard(typeof(DiceomancerCharacter), 4)]
public class DefendBuilder() :
    ModCardTemplate(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];


    public override bool GainsBlock => true;

    public async Task OnRightClick(ModRightClickExecutionContext context)
    {
        await CardCmd.Discard(context.PlayerChoiceContext, (CardModel)context.Model);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5m, ValueProp.Move)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        
    }


    protected override void OnUpgrade()
    {
        // DynamicVars.Damage.UpgradeValueBy(4); 
        EnergyCost.UpgradeBy(-1);
    }
}