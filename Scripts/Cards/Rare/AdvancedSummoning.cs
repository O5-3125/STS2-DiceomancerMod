using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers.Elements;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;

namespace Diceomancer.Scripts.Cards.Rare;

[RegisterCard(typeof(DiceomancerCardPool))]
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
        new PowerVar<FireElement>(2),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (int i = 0; i < DynamicVars["WaterElement"].IntValue; i++)
        {
            await PowerCmd.Apply<WaterElement>(choiceContext, base.Owner.Creature, 4,
                base.Owner.Creature, this);
        }

        for (int i = 0; i < DynamicVars["FireElement"].IntValue; i++)
        {
            await PowerCmd.Apply<FireElement>(choiceContext, base.Owner.Creature, 3,
                base.Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["WaterElement"].UpgradeValueBy(1m);
        base.DynamicVars["FireElement"].UpgradeValueBy(1m);
    }
}