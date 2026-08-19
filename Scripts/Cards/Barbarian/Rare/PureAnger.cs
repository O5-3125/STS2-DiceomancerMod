using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero.Barbarian;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Barbarian.Rare;

[RegisterCard(typeof(BarbarianCardPool))]
public class PureAnger : ModCardTemplate
{
    public PureAnger() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override bool HasEnergyCostX => true;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [MyKeywords.Bonus];

    public override int ModifyXValue(CardModel card, int originalValue)
    {
        return originalValue + (IsUpgraded ? 1 : 0);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var xValue = ResolveEnergyXValue();
        if (xValue <= 0) return;

        await BarbarianCardUtils.ChannelEmotionOrbs(choiceContext, Owner, xValue);
    }

    protected override void OnUpgrade()
    {
    }
}