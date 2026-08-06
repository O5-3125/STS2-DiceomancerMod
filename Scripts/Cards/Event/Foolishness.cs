using Diceomancer.Scripts.Common;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Event;

[RegisterCard(typeof(EventCardPool))]
public class Foolishness() : ModCardTemplate(0, CardType.Skill, CardRarity.Event, TargetType.Self)

{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(6)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        List<CardModel> cardsIn = (from c in PileType.Draw.GetPile(Owner).Cards
            orderby c.Rarity, c.Id
            select c).ToList();
        var list = (await CardSelectCmd.FromSimpleGrid(choiceContext, cardsIn, Owner,
                new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 0, DynamicVars.Cards.IntValue)))
            .ToList();


        await CardCmd.Discard(choiceContext, list);
    }

    protected override void OnUpgrade()
    {
        this.AddModKeyword(MyKeywords.Bonus);
    }
}