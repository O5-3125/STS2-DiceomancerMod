using STS2RitsuLib.Scaffolding.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Diceomancer.Scripts.Cards.Rare;

// [RegisterCard(typeof(DiceomancerCardPool))]
public class Lotus() : ModCardTemplate(5, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new("Lotus", 5)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    // ���ʱ��Ч���߼�?
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(this.Owner.PlayerCombatState, "this.Owner.PlayerCombatState");

        List<CardModel> list = this.Owner.PlayerCombatState.AllCards.ToList();
        foreach (var item in list)
        {
            var keyList = item.DynamicVars.Keys;
            foreach (var key in keyList) item.DynamicVars[key].BaseValue += DynamicVars["Lotus"].BaseValue;
        }
    }

    // �������Ч���߼�?
    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}