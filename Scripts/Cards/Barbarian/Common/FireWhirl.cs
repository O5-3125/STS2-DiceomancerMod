using Diceomancer.Scripts.Cards.Token;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero.Barbarian;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Barbarian.Common;

[RegisterCard(typeof(BarbarianCardPool))]
public class FireWhirl() : ModCardTemplate(1, CardType.Skill, CardRarity.Common, TargetType.AllEnemies)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<BurnPower>(5),
        new CardsVar(2)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromCard<Flame>(),
        HoverTipFactory.FromPower<BurnPower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);

        await PowerCmd.Apply<BurnPower>(choiceContext, CombatState.HittableEnemies,
            DynamicVars["BurnPower"].IntValue, Owner.Creature, this);

        await CardPileCmd.AddToCombatAndPreview<Flame>(Owner.Creature, PileType.Hand, DynamicVars.Cards.IntValue, null);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(MyKeywords.Storm);
    }
}