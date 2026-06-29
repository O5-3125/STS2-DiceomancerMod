using STS2RitsuLib.Scaffolding.Content;
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

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
[RegisterCharacterStarterCard(typeof(DiceomancerCharacter))]
public class WhatIsThis() : ModCardTemplate(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];


    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3),
        new EnergyVar(0),
        new("selectCount", 1)
    ];

    // ���ʱ��Ч���߼�?
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(base.DynamicVars.Energy.IntValue, base.Owner);
        //
        //
        //
        //
        // ���н�ɫ�Ŀ����б�
        List<CardPoolModel> cardPoolList = base.Owner.UnlockState.CharacterCardPools.ToList();
        // ����ְҵ���б�
        IEnumerable<CardModel> cardModelList = from c in cardPoolList.SelectMany(c =>
                c.GetUnlockedCards(base.Owner.UnlockState, base.Owner.RunState.CardMultiplayerConstraint)
            )
            where c.Rarity != CardRarity.Rare
            select c;
        // if (player != base.Owner)
        // {
        //     return;
        // }
        // List<CardModel> list = CardFactory.GetDistinctForCombat(base.Owner,
        //     this.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardGeneration).ToList();


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

        // foreach (CardModel item in list)
        // {
        //     CardCmd.ApplyKeyword(item, CardKeyword.Retain);
        // }

        foreach (var item2 in await CardSelectCmd.FromSimpleGrid(choiceContext, list, base.Owner,
                     new CardSelectorPrefs(this.SelectionScreenPrompt, 0, this.DynamicVars["selectCount"].IntValue)))
            await CardPileCmd.AddGeneratedCardToCombat(item2, PileType.Hand, base.Owner);

        // ��������
        // CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(cardModel, PileType.Deck));
    }

    // �������Ч���߼�?
    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }
}