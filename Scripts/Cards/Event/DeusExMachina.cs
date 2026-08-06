using Diceomancer.Scripts.Common;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Event;

[RegisterCard(typeof(EventCardPool))]
public class DeusExMachina() : ModCardTemplate(2, CardType.Attack, CardRarity.Event, TargetType.AllEnemies)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [MyKeywords.Limited];

    protected override bool ShouldGlowGoldInternal =>
        Owner.PlayerCombatState?.TurnNumber > DynamicVars["Turns"].BaseValue;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new("Turns", 5),
        new DamageVar("Before", 12, ValueProp.Move),
        new DamageVar("After", 50, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var damage = Owner.PlayerCombatState?.TurnNumber < DynamicVars["Turns"].BaseValue
            ? DynamicVars["Before"].IntValue
            : DynamicVars["After"].IntValue;

        await DamageCmd.Attack(damage)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(Owner.Creature.CombatState)
            .Execute(choiceContext);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["Turns"].UpgradeValueBy(-1);
        DynamicVars["Before"].UpgradeValueBy(3);
        DynamicVars["After"].UpgradeValueBy(10);
    }
}