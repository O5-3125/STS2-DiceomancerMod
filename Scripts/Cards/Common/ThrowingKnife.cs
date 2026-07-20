using Diceomancer.Scripts.Common;
using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Common;

[RegisterCard(typeof(DiceomancerCardPool))]
public class ThrowingKnife() : ModCardTemplate(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3m, ValueProp.Move),
        new PowerVar<BleedPower>(1m)
    ];


    public override IEnumerable<CardKeyword> CanonicalKeywords => [MyKeywords.Bonus,CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<BleedPower>(),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        await PowerCmd.Apply<BleedPower>(choiceContext, cardPlay.Target, base.DynamicVars["BleedPower"].BaseValue,
            base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Damage.UpgradeValueBy(1m);
        base.DynamicVars["BleedPower"].UpgradeValueBy(1m);
    }
}