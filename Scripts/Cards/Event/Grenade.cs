using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Event;

[RegisterCard(typeof(ColorlessCardPool))]
public class Grenade() : ModCardTemplate(1, CardType.Attack, CardRarity.Event, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(30, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(base.Owner.Creature.CombatState, "base.Owner.Creature.CombatState");

        Creature creature = base.Owner.Creature;
        DamageVar damage = base.DynamicVars.Damage;
        await CreatureCmd.Damage(choiceContext,
            base.Owner.Creature.CombatState.Creatures.Where((Creature c) => !c.IsPet)
            , damage.BaseValue, damage.Props, creature, this);
    }

    protected override void OnUpgrade()
    {
        this.AddKeyword(CardKeyword.Retain);
        DynamicVars.Damage.UpgradeValueBy(20);
    }
}