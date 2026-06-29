using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class TensionAndRelaxation()
    : ModCardTemplate(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


    // ���ʱ��Ч���߼�?
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (var i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            var cardAttack = PileType.Draw.GetPile(base.Owner)
                .Cards.Where(c => c.Type == CardType.Attack).ToList()
                .StableShuffle(base.Owner.RunState.Rng.Shuffle)
                .FirstOrDefault();
            var cardSkill = PileType.Draw.GetPile(base.Owner)
                .Cards.Where(c => c.Type == CardType.Skill).ToList()
                .StableShuffle(base.Owner.RunState.Rng.Shuffle)
                .FirstOrDefault();

            if (cardSkill != null) await CardPileCmd.Add(cardSkill, PileType.Hand);

            if (cardAttack != null) await CardPileCmd.Add(cardAttack, PileType.Hand);
        }
    }

    // �������Ч���߼�?
    protected override void OnUpgrade()
    {
        this.AddKeyword(CardKeyword.Retain);
    }
}