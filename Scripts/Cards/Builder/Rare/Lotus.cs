using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Rare;

[RegisterCard(typeof(BuilderCardPool))]
public class Lotus() : ModCardTemplate(5, CardType.Skill, CardRarity.Rare, TargetType.Self)
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

        ModifyCardCmd.ModifyCardListDynamicVarsAdditive(list, (int)DynamicVars["Lotus"].BaseValue);

        // foreach (var item in list)
        // {
        //     var keyList = item.DynamicVars.Keys;
        //     foreach (var key in keyList) item.DynamicVars[key].BaseValue += DynamicVars["Lotus"].BaseValue;
        // }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}