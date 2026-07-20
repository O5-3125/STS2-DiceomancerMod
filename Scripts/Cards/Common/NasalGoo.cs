using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Common;

[RegisterCard(typeof(DiceomancerCardPool))]
public class NasalGoo()
    : ModCardTemplate(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<WeakPower>(2m),
        new PowerVar<PowerlessPower>(3)
    ];

    // 通过HoverTipFactory添加各种提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<PowerlessPower>(),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target,
            base.DynamicVars.Weak.BaseValue, base.Owner.Creature, this);

        await PowerCmd.Apply<PowerlessPower>(choiceContext, cardPlay.Target,
            base.DynamicVars["PowerlessPower"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["WeakPower"].UpgradeValueBy(2);
        this.DynamicVars["PowerlessPower"].UpgradeValueBy(1);
    }
}