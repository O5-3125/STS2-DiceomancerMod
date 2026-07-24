using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Rare;

[RegisterCard(typeof(DiceomancerCardPool))]
public class DeusExMachina() : ModCardTemplate(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [MyKeywords.Limited];

    protected override bool ShouldGlowGoldInternal =>
        Owner.PlayerCombatState?.TurnNumber > DynamicVars["Turns"].BaseValue;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Turns", 5),
        new DamageVar("Before", 12, ValueProp.Move),
        new DamageVar("After", 50, ValueProp.Move),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int damage;
        if (Owner.PlayerCombatState?.TurnNumber < DynamicVars["Turns"].BaseValue)
        {
            damage = DynamicVars["Before"].IntValue;
        }
        else
        {
            damage = DynamicVars["After"].IntValue;
        }

        await DamageCmd.Attack(damage)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(Owner.Creature.CombatState)
            .Execute(choiceContext);

        // if (Owner.PlayerCombatState?.TurnNumber < DynamicVars["Turns"].BaseValue)
        // {
        //     await DamageCmd.Attack(DynamicVars["Before"].IntValue)
        //         .FromCard(this, cardPlay)
        //         .TargetingAllOpponents(Owner.Creature.CombatState)
        //         .Execute(choiceContext);
        // }
        // else
        // {
        //     await DamageCmd.Attack(DynamicVars["After"].IntValue)
        //         .FromCard(this, cardPlay)
        //         .TargetingAllOpponents(Owner.Creature.CombatState)
        //         .Execute(choiceContext);
        // }
    }


    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(2);
    }
}