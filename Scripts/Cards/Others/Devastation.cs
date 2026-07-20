using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Rare;

// [RegisterCard(typeof(DiceomancerCardPool))]
public class Devastation() : ModCardTemplate(2, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(5), new EnergyVar(1)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards =
            (await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner)).Where(c =>
                c.Type != CardType.Attack).ToList();
        await CardCmd.Discard(choiceContext, cards);
        await PlayerCmd.GainEnergy(
            (DynamicVars.Cards.IntValue - cards.Count) * DynamicVars.Energy.IntValue,
            base.Owner);
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(2m);
    }
}