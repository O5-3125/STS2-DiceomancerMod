using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero.Barbarian;
using Diceomancer.Scripts.Powers.Berserker;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Barbarian.Uncommon;

[RegisterCard(typeof(BarbarianCardPool))]
public class BlockWithFace() : ModCardTemplate(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, MyKeywords.Wild];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<InjuryBlock>(1),
        new CardsVar(3)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<InjuryBlock>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<InjuryBlock>(choiceContext, Owner.Creature,
            DynamicVars["InjuryBlock"].IntValue, Owner.Creature, this);

        await CardPileCmd.AddToCombatAndPreview<Dazed>(Owner.Creature, PileType.Draw,
            DynamicVars.Cards.IntValue, null);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}