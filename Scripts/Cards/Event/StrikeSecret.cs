using STS2RitsuLib.Scaffolding.Content;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Event;

[RegisterCard(typeof(EventCardPool))]
public class StrikeSecret() : ModCardTemplate(3, CardType.Attack, CardRarity.Event, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(5)];

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var distinctForCombat = CardFactory.GetDistinctForCombat(Owner,
            from c in ModelDb.AllCards
            where c.Tags.Contains(CardTag.Strike) && c is not StrikeSecret
            select c,
            DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardGeneration);

        foreach (var item in distinctForCombat.ToList())
            await CardCmd.AutoPlay(choiceContext, item.CreateDupe(), null);
    }


    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(5);
        RemoveKeyword(CardKeyword.Exhaust);
    }
}