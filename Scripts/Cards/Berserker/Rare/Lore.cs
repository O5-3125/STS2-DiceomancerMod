using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero.Berserker;
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
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Berserker.Rare;

[RegisterCard(typeof(BerserkerCardPool))]
public class Lore() : ModCardTemplate(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

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
        var cardPoolList = Owner.UnlockState.CharacterCardPools.ToList();

        IEnumerable<CardModel> cardModelList = from c in cardPoolList.SelectMany(c =>
                c.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
            )
            where c.Rarity == CardRarity.Rare
            select c;
        var list = CardFactory.GetDistinctForCombat(Owner,
            cardModelList,
            Math.Min(cardModelList.Count(), DynamicVars.Cards.IntValue),
            Owner.RunState.Rng.CombatCardGeneration).ToList();
        if (list.Count == 0)
        {
            var text = "ChoicesParadox generated no cards for selection. Returning early to prevent softlock.";
            Log.Error(text);
            SentryService.CaptureException(new SoftlockException(text));
            return;
        }

        foreach (var item2 in await CardSelectCmd.FromSimpleGrid(choiceContext, list, Owner,
                     new CardSelectorPrefs(SelectionScreenPrompt, 0, DynamicVars["selectCount"].IntValue)))
            await CardCmd.AutoPlay(choiceContext, item2.CreateDupe(Owner), null);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}