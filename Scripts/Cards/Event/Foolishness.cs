using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Foolishness() : ModCardTemplate(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)

{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(6)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
        List<CardModel> cardsIn = (from c in PileType.Draw.GetPile(base.Owner).Cards
            orderby c.Rarity, c.Id
            select c).ToList();
        var list = (await CardSelectCmd.FromSimpleGrid(choiceContext, cardsIn, base.Owner,
                new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, base.DynamicVars.Cards.IntValue)))
            .ToList();


        await CardCmd.Discard(choiceContext, list);
    }

    protected override void OnUpgrade()
    {
        this.AddModKeyword(MyKeywords.Bonus);
    }
}