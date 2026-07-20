using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.CardPool;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Upgrade;

[RegisterCard(typeof(UpgradeCardPool))]
public class LifeTakerThreeThousand()
    : ModCardTemplate(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Buff", 5),
        new RepeatVar(5)
    ];
    
    public override bool GainsBlock => true;
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (var i = 0; i < DynamicVars.Repeat.IntValue; i++)
        {
            await DiceomancerCardCmd.ApplyRandomBuff(choiceContext, Owner, Owner.Creature, Owner.Creature, this,
                DynamicVars["Buff"].IntValue);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}