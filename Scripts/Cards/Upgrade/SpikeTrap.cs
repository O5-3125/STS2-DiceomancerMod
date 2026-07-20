using Diceomancer.Scripts.Hero.CardPool;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Upgrade;

[RegisterCard(typeof(UpgradeCardPool))]
public sealed class SpikeTrap()
    : ModCardTemplate(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<PlatingPower>(5),
        new PowerVar<ThornsPower>(5),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature,
            DynamicVars["PlatingPower"].IntValue, Owner.Creature, this);

        await PowerCmd.Apply<ThornsPower>(choiceContext, Owner.Creature,
            DynamicVars["ThornsPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["ThornsPower"].UpgradeValueBy(1);
        DynamicVars["PlatingPower"].UpgradeValueBy(1);
        AddKeyword(CardKeyword.Retain);
    }
}