using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero.CardPool;
using Diceomancer.Scripts.Powers.Elements;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Upgrade;

[RegisterCard(typeof(UpgradeCardPool))]
public class ElementEmergence() : ModCardTemplate(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new SummonVar(4)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<FireElement>(),
        HoverTipFactory.FromPower<WaterElement>(),
        HoverTipFactory.FromPower<EarthElement>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (var i = 0; i < DynamicVars.Summon.IntValue; i++)
        {
            await ElementCmd.SummonRandomBasicElement(choiceContext, Owner.Creature, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Summon.UpgradeValueBy(1);
    }
}