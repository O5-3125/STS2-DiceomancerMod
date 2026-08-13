using Diceomancer.Scripts.Common.Patches;
using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Common;

[RegisterCard(typeof(BuilderCardPool))]
public class ChargedStrike() : ModCardTemplate(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6, ValueProp.Move),
        new("ChargeGain", 2)
    ];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var played = cardPlay.Card;
        if (played == this || played.Owner != Owner) return;
        if (!ChargedStrikeNeighborTracker.GetNeighbors(played).Contains(this)) return;

        DynamicVars.Damage.BaseValue += DynamicVars["ChargeGain"].IntValue;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ChargeGain"].UpgradeValueBy(1);
    }
}
