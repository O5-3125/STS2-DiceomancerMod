using Diceomancer.Scripts.Hero.Berserker;
using Diceomancer.Scripts.Powers.Berserker;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Berserker.Rare;
// TODO 大爆裂术
// [RegisterCard(typeof(BerserkerCardPool))]
public class TheExplosionSpell() : ModCardTemplate(5, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(99, ValueProp.Move),
        new PowerVar<EnergyCapPower>(3)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(CombatState)
            .Execute(choiceContext);

        await PowerCmd.Apply<EnergyCapPower>(choiceContext, Owner.Creature,
            DynamicVars["EnergyCapPower"].IntValue, Owner.Creature, this);

        await CardCmd.Discard(choiceContext, PileType.Hand.GetPile(Owner).Cards);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(51);
    }
}