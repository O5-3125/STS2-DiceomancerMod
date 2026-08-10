using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Others;

// [RegisterCard(typeof(BuilderCardPool))]
public class WorkOvertime()
    : ModCardTemplate(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


    // protected override IEnumerable<DynamicVar> CanonicalVars =>
    // [
    //     new HpLossVar(3m),
    //     // new EnergyVar(1),
    //     new CardsVar(1),
    //     new PowerVar<TechPower>(1)
    // ];
    //
    // protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    // [
    //     HoverTipFactory.FromPower<TechPower>()
    // ];
    //
    // protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    // {
    //     await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue,
    //         ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
    //
    //
    //     await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    //
    //     await PowerCmd.Apply<TechPower>(choiceContext,
    //         Owner.Creature, DynamicVars["TechPower"].IntValue, Owner.Creature, this);
    // }

    protected override void OnUpgrade()
    {
        DynamicVars.HpLoss.UpgradeValueBy(-2);
        DynamicVars["TechPower"].UpgradeValueBy(1);
    }
}