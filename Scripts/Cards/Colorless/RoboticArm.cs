using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Colorless;

[RegisterCard(typeof(ColorlessCardPool))]
public class RoboticArm() : ModCardTemplate(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<MaxHandSize>(36)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<MaxHandSize>(choiceContext, Owner.Creature,
            DynamicVars["MaxHandSize"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}