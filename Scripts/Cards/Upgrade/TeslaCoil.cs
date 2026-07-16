using Diceomancer.Scripts.Hero.CardPool;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Upgrade;

// todo 特斯拉线圈
[RegisterCard(typeof(UpgradeCardPool))]
public sealed class TeslaCoil()
    : ModCardTemplate(1, CardType.Attack, CardRarity.Basic, TargetType.AllEnemies)
{
    // protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    // [
    //     HoverTipFactory.FromCard<PipeGun>(),
    // ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(12, ValueProp.Move),
        new PowerVar<TaintedPower>(1),
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingRandomOpponents(Owner.Creature.CombatState)
            .Execute(choiceContext);

        await PowerCmd.Apply<TaintedPower>(choiceContext, cardPlay.Target,
            DynamicVars["TaintedPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(4m);
    }
}