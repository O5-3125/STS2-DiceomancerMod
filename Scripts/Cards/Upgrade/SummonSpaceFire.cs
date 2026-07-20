using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.CardPool;
using Diceomancer.Scripts.Powers.Elements;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Upgrade;

[RegisterCard(typeof(UpgradeCardPool))]
public class SummonSpaceFire() : ModCardTemplate(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new SummonVar(20),
        new DamageVar(6, ValueProp.Unpowered),
        new RepeatVar(6)
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    // 通过HoverTipFactory添加各种提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<SpaceFireElement>(),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var spaceFireElement =
            await PowerCmd.Apply<SpaceFireElement>(choiceContext, base.Owner.Creature, DynamicVars.Summon.IntValue,
                base.Owner.Creature, this);
        if (spaceFireElement != null)
        {
            spaceFireElement.DynamicVars.Damage.BaseValue = base.DynamicVars.Damage.BaseValue;
            spaceFireElement.DynamicVars.Repeat.BaseValue = base.DynamicVars.Repeat.BaseValue;
        }
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}