using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class ElasticDefence() : ModCardTemplate(2, CardType.Power, CardRarity.Uncommon, TargetType.Self, true)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Powers.ElasticDefence>(5m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        await PowerCmd.Apply<Powers.ElasticDefence>(choiceContext, base.Owner.Creature,
            base.DynamicVars["ElasticDefence"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ElasticDefence"].UpgradeValueBy(1);
    }
}