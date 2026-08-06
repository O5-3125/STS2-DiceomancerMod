using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Basic;

[RegisterCard(typeof(BuilderCardPool))]
[RegisterCharacterStarterCard(typeof(Hero.Builder.Builder))]
public sealed class Pipe()
    : UpgradeTemplate<PipeGun>(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy, 3)
{
    public override CardAssetProfile AssetProfile => new(
        PortraitPath:$"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
        new DamageVar(4, ValueProp.Move),
        new PowerVar<WeakPower>(1)
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target,
            DynamicVars.Weak.IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}