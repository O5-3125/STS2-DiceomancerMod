using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Others;

// [RegisterCard(typeof(BuilderCardPool))]
public class ForgeTemplate() : ModCardTemplate(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override bool GainsBlock => true;

    // protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    // [
    //     HoverTipFactory.FromPower<TechPower>()
    // ];
    //
    // protected override IEnumerable<DynamicVar> CanonicalVars =>
    // [
    //     new BlockVar(8, ValueProp.Move),
    //     new PowerVar<TechPower>(1)
    // ];
    //
    // protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    // {
    //     await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    //
    //     await PowerCmd.Apply<TechPower>(choiceContext, Owner.Creature,
    //         DynamicVars["TechPower"].IntValue, Owner.Creature, this);
    // }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3);
        DynamicVars["TechPower"].UpgradeValueBy(1);
    }
}