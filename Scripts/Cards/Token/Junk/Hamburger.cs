using Diceomancer.Scripts.Hero.CardPool;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.Berserker;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Token.Junk;

[RegisterCard(typeof(JunkCardPool))]
public class Hamburger() : ModCardTemplate(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override int MaxUpgradeLevel => 0;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<Excitement>(2),
        new PowerVar<FortifiedPower>(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<Excitement>(choiceContext, Owner.Creature,
            DynamicVars["Excitement"].IntValue, Owner.Creature, this);
        await PowerCmd.Apply<FortifiedPower>(choiceContext, Owner.Creature,
            DynamicVars["FortifiedPower"].IntValue, Owner.Creature, this);
    }
}
