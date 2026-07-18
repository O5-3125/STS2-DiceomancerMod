using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Exceptions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Rare;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Lore() : ModCardTemplate(1, CardType.Skill, CardRarity.Rare, TargetType.Self, true)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
        MyKeywords.Limited
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3),
        new("selectCount", 1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {

        List<CardPoolModel> cardPoolList = base.Owner.UnlockState.CharacterCardPools.ToList();

        IEnumerable<CardModel> cardModelList = from c in cardPoolList.SelectMany(c =>
                c.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
            )
            where c.Rarity == CardRarity.Rare
            select c;
        var list = CardFactory.GetDistinctForCombat(base.Owner,
            cardModelList,
            Math.Min(cardModelList.Count(), this.DynamicVars.Cards.IntValue),
            base.Owner.RunState.Rng.CombatCardGeneration).ToList();
        if (list.Count == 0)
        {
            var text = "ChoicesParadox generated no cards for selection. Returning early to prevent softlock.";
            Log.Error(text);
            SentryService.CaptureException(new SoftlockException(text));
            return;
        }

        foreach (var item2 in await CardSelectCmd.FromSimpleGrid(choiceContext, list, base.Owner,
                     new CardSelectorPrefs(this.SelectionScreenPrompt, 0, this.DynamicVars["selectCount"].IntValue)))
            await CardCmd.AutoPlay(choiceContext, item2.CreateDupe(Owner), null);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}