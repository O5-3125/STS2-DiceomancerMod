using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Powers.NormalityPower;
using STS2RitsuLib.Scaffolding.Content;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Event;

[RegisterCard(typeof(EventCardPool))]
public class InvincibleStrike() : ModCardTemplate(3, CardType.Attack, CardRarity.Event, TargetType.RandomEnemy)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5m, ValueProp.Move), 
        new RepeatVar(5), 
        new("Evade", 3M)
    ];

    
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<EvadePower>()
    ];
    

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState, "base.CombatState");

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingRandomOpponents(CombatState)
            .WithHitCount(DynamicVars.Repeat.IntValue)
            .Execute(choiceContext);

        await PowerCmd.Apply<EvadePower>(choiceContext, Owner.Creature, DynamicVars["Evade"].BaseValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
        DynamicVars.Repeat.UpgradeValueBy(1);
        DynamicVars["Evade"].UpgradeValueBy(2);
    }
}