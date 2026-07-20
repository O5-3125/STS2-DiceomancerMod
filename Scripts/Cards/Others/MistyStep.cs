using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Common;

// [RegisterCard(typeof(DiceomancerCardPool))]
public class MistyStep() : ModCardTemplate(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new("Evade", 2M),
    ];
    // 通过HoverTipFactory添加各种提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<EvadePower>(),
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<EvadePower>(choiceContext,
            base.Owner.Creature,
            DynamicVars["Evade"].IntValue,
            base.Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Evade"].UpgradeValueBy(1);
    }
}