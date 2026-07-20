using Diceomancer.Scripts.Hero;
using STS2RitsuLib.Scaffolding.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Rare;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Lotus() : ModCardTemplate(3, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new("Lotus", 5)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(Owner.PlayerCombatState);

        var list = Owner.PlayerCombatState.AllCards.ToList();
        foreach (var item in list)
        {
            var keyList = item.DynamicVars.Keys;
            foreach (var key in keyList) item.DynamicVars[key].BaseValue += DynamicVars["Lotus"].BaseValue;
        }
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}