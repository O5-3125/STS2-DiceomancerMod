using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Hero.Builder;
using Diceomancer.Scripts.Powers.Elements;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Rare;

[RegisterCard(typeof(BuilderCardPool))]
public class AdvancedSummoning()
    : UpgradeTemplate<SummonSpaceFire>(2, CardType.Skill, CardRarity.Rare, TargetType.Self, 4)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<IHoverTip> OwnAdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<FireElement>(),
        HoverTipFactory.FromPower<WaterElement>()
    ];

    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
        new PowerVar<WaterElement>(2),
        new PowerVar<FireElement>(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (var i = 0; i < DynamicVars["WaterElement"].IntValue; i++)
            await PowerCmd.Apply<WaterElement>(choiceContext, Owner.Creature, 4,
                Owner.Creature, this);

        for (var i = 0; i < DynamicVars["FireElement"].IntValue; i++)
            await PowerCmd.Apply<FireElement>(choiceContext, Owner.Creature, 3,
                Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["WaterElement"].UpgradeValueBy(1m);
        DynamicVars["FireElement"].UpgradeValueBy(1m);
    }
}