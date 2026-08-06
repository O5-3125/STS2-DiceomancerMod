using Diceomancer.Scripts.Hero.Builder;
using Diceomancer.Scripts.Powers.Elements;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Uncommon;

[RegisterCard(typeof(BuilderCardPool))]
public class SummonEarth() : ModCardTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new SummonVar(3),
        new EnergyVar(1)
    ];

    // 通过HoverTipFactory添加各种提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<EarthElement>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var earthElement =
            await PowerCmd.Apply<EarthElement>(choiceContext, Owner.Creature, DynamicVars.Summon.IntValue,
                Owner.Creature, this);
        if (earthElement != null) earthElement.DynamicVars.Energy.BaseValue = DynamicVars.Energy.BaseValue;
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}