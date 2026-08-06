using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Rare;

[RegisterCard(typeof(BuilderCardPool))]
public class Spacecaps()
    : UpgradeTemplate<DysonSphere>(1, CardType.Skill, CardRarity.Rare, TargetType.Self, 7)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
        new CardsVar(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var attackCards = PileType.Draw.GetPile(Owner)
            .Cards.Where(c => c.Type == CardType.Attack)
            .ToList()
            .StableShuffle(Owner.RunState.Rng.Shuffle)
            .Take(DynamicVars.Cards.IntValue)
            .ToList();

        var skillCards = PileType.Draw.GetPile(Owner)
            .Cards.Where(c => c.Type == CardType.Skill)
            .ToList()
            .StableShuffle(Owner.RunState.Rng.Shuffle)
            .Take(DynamicVars.Cards.IntValue)
            .ToList();

        if (attackCards.Count != 0)
            await CardPileCmd.Add(attackCards, PileType.Hand);

        if (skillCards.Count != 0)
            await CardPileCmd.Add(skillCards, PileType.Hand);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}