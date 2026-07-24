using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.CardPool;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interactions.RightClick;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Upgrade;

[RegisterCard(typeof(UpgradeCardPool))]
public class Dawn()
    : ModCardTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<StrengthPower>(2),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature,
            DynamicVars["StrengthPower"].IntValue, Owner.Creature, this);

        // if (!IsUpgraded)
        // {
        //     await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target,
        //         -DynamicVars["StrengthPower"].IntValue, Owner.Creature, this);
        // }
        // else
        // {
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature.CombatState.HittableEnemies,
                -DynamicVars["StrengthPower"].IntValue, Owner.Creature, this);
        // }
    }
    //
    // public override TargetType TargetType => Target;
    // private TargetType Target { get; set; } = TargetType.AnyEnemy;

    protected override void OnUpgrade()
    {
        // Target = TargetType.AllEnemies;
        DynamicVars["StrengthPower"].UpgradeValueBy(1);
        
    }
}