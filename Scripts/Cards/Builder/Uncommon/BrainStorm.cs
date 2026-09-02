using Diceomancer.Scripts.Hero.Builder;
using Diceomancer.Scripts.Hero.CardPool;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Uncommon;

[RegisterCard(typeof(BuilderCardPool))]
public class BrainStorm() : ModCardTemplate(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3),
        new CardsVar("Select", 1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cards = CardFactory.GetForCombat(Owner,
            Owner.Character.CardPool.AllCards,
            DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardGeneration).ToList();
        if (cards.Count == 0) return;

        if (IsUpgraded)
        {
            foreach (var model in cards)
            {
                CardCmd.Upgrade(model);
            }
        }


        var selected = (await CardSelectCmd.FromSimpleGrid(
            choiceContext, cards, Owner,
            new CardSelectorPrefs(SelectionScreenPrompt, 0, DynamicVars["Select"].IntValue))).ToList();


        await CardPileCmd.AddGeneratedCardsToCombat(selected, PileType.Hand, Owner);
    }

    protected override void OnUpgrade()
    {
    }
}