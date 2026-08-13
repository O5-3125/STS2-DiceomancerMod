using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.Berserker;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Berserker.Basic;

[RegisterCard(typeof(BerserkerCardPool))]
public class RageStrike : ModCardTemplate, ISecondaryResourceHookListener
{
    public RageStrike() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        // 打出此牌需要消耗 2 点怒火
        this.SecondaryCosts().Set(Rage.Id, 2);
    }

    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10, ValueProp.Move),
        SecondaryResourceVars.For("Rage", Rage.Id, 2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // ArgumentNullException.ThrowIfNull(cardPlay.Target);
        //
        //
        // await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
        //     .FromCard(this, cardPlay)
        //     .Targeting(cardPlay.Target)
        //     .Execute(choiceContext);

        await SecondaryResourceCmd.Gain(Owner, Rage.Id, 2);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
    }
}