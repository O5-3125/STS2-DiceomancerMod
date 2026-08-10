using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Others;

// [RegisterCard(typeof(BuilderCardPool))]
public class Reuse()
    : ModCardTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    // protected override IEnumerable<DynamicVar> CanonicalVars =>
    // [
    //     new PowerVar<TechPower>(2),
    //     new CardsVar(1)
    // ];
    // protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    // [
    //     HoverTipFactory.FromPower<TechPower>()
    // ];
    // protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    // {
    //     var selection = (await CardSelectCmd.FromHand(choiceContext, Owner,
    //         new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, DynamicVars.Cards.IntValue), null, this));
    //
    //     foreach (var cardModel in selection)
    //     {
    //         await CardCmd.Exhaust(choiceContext, cardModel);
    //     }
    //
    //     await PowerCmd.Apply<TechPower>(choiceContext, Owner.Creature,
    //         DynamicVars["TechPower"].IntValue, Owner.Creature, this);
    // }

    protected override void OnUpgrade()
    {
        DynamicVars["TechPower"].UpgradeValueBy(1);
    }
}