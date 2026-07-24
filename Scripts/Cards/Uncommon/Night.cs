using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Night()
    : UpgradeTemplate<Dawn>(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, 3)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
        new PowerVar<BlindPower>(1),
    ];

    protected override IEnumerable<IHoverTip> OwnAdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<BlindPower>(),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!IsUpgraded)
        {
            await PowerCmd.Apply<BlindPower>(choiceContext, cardPlay.Target,
                DynamicVars["BlindPower"].IntValue, Owner.Creature, this);
        }
        else
        {
            await PowerCmd.Apply<BlindPower>(choiceContext, Owner.Creature.CombatState.HittableEnemies,
                DynamicVars["BlindPower"].IntValue, Owner.Creature, this);
        }
    }

    public override TargetType TargetType => Target;
    private TargetType Target { get; set; } = TargetType.AnyEnemy;

    protected override void OnUpgrade()
    {
        Target = TargetType.AllEnemies;
    }
}