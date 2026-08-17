using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.Berserker;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Berserker.Rare;

[RegisterCard(typeof(BerserkerCardPool))]
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

        await SecondaryResourceCmd.Gain(Owner, Rage.Id, xValue, this);
    }

    protected override void OnUpgrade()
    {
    }
}