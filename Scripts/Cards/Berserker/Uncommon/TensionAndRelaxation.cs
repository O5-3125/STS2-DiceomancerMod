using Diceomancer.Scripts.Hero.Berserker;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Berserker.Uncommon;

[RegisterCard(typeof(BerserkerCardPool))]
public class TensionAndRelaxation()
    : ModCardTemplate(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (var i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            var cardAttack = PileType.Draw.GetPile(Owner)
                .Cards.Where(c => c.Type == CardType.Attack).ToList()
                .StableShuffle(Owner.RunState.Rng.Shuffle)
                .FirstOrDefault();
            var cardSkill = PileType.Draw.GetPile(Owner)
                .Cards.Where(c => c.Type == CardType.Skill).ToList()
                .StableShuffle(Owner.RunState.Rng.Shuffle)
                .FirstOrDefault();

            if (cardSkill != null) await CardPileCmd.Add(cardSkill, PileType.Hand);

            if (cardAttack != null) await CardPileCmd.Add(cardAttack, PileType.Hand);
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}