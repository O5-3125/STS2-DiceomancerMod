using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero.Berserker;
using Diceomancer.Scripts.Powers.NormalityPower;
using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Berserker.Rare;

[RegisterCard(typeof(BerserkerCardPool))]
public class BorrowedTime() : ModCardTemplate(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var injury = Owner.Creature.GetPowerAmount<Injury>();
        var halfAmount = injury / 2m;
        if (halfAmount < 1m) return;

        await BerserkerCardUtils.HealInjury(choiceContext, Owner.Creature, halfAmount);
        await CreatureCmd.Heal(Owner.Creature, halfAmount, true);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}