using STS2RitsuLib.Scaffolding.Content;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Rare;

[RegisterCard(typeof(ColorlessCardPool))]
public class CardOfManyThings() : ModCardTemplate(3, CardType.Skill, CardRarity.Rare, TargetType.RandomEnemy)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(10)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var distinctForCombat = CardFactory.GetDistinctForCombat(Owner,
            from c in ModelDb.AllCards
            // where (c.Rarity==CardRarity.Basic)
            select c,
            DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardGeneration);

        foreach (var item in distinctForCombat.ToList())
            await CardCmd.AutoPlay(choiceContext, item.CreateDupe(Owner), null);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(5); 
        // RemoveKeyword(CardKeyword.Exhaust);
    }
}