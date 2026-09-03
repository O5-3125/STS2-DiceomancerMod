using Diceomancer.Scripts.Hero.Barbarian;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.Berserker;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Barbarian.Common;

[RegisterCard(typeof(BarbarianCardPool))]
public class Payback() : ModCardTemplate(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    
    
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<Injury>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        var injury = Owner.Creature.GetPowerAmount<Injury>();
        if (injury <= 0) return;

        await DamageCmd.Attack(injury)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}