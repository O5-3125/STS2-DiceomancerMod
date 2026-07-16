using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Singleton;

// 注册单例
[RegisterSingleton]
public class TestSingleton : SingletonModel
{
    public TestSingleton()
    {
        // 获得监听CombatState钩子的能力
        ModHelper.SubscribeForCombatStateHooks(Id.Entry, state => [this]);
        // 获得监听RunState钩子的能力
        ModHelper.SubscribeForRunStateHooks(Id.Entry, state => [this]);
    }

    // 没有用但是要写
    public override bool ShouldReceiveCombatHooks => true;


    // 抽到的逻辑
    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        // 附赠
        if (card.Keywords.Contains(MyKeywords.Bonus)) await CardPileCmd.Draw(choiceContext, 1m, card.Owner);

        // 混乱
        if (card.Keywords.Contains(MyKeywords.Chaos))
        {
            var keyList = card.DynamicVars.Keys.ToList();
            foreach (var key in keyList)
                card.DynamicVars[key].BaseValue = RandomCmd.CheckD6(card.Owner);
        }
    }

    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 进化
        if (cardPlay.Card.Tags.Contains(MyTags.Evolution.GetModCardTag()))
        {
            var keyList = cardPlay.Card.DynamicVars.Keys;
            foreach (var key in keyList)
                if (key != "Evolution")
                    cardPlay.Card.DynamicVars[key].BaseValue += cardPlay.Card.DynamicVars["Evolution"].BaseValue;
        }

        // 打出限制牌获得疲劳
        if (cardPlay.Card.Keywords.Contains(MyKeywords.Limited))
            await PowerCmd.Apply<Fatigue>(
                choiceContext, cardPlay.Card.Owner.Creature, 1, null, null);


        // 打出获得幻影复制
        if (cardPlay.Card.Keywords.Contains(MyKeywords.Phantom))
        {
            var cardModel = cardPlay.Card.CreateClone(); // 获得复制
            cardModel.EnergyCost.AddThisCombat(-1); // 减一费
            cardModel.AddKeyword(CardKeyword.Exhaust); // 消耗
            cardModel.RemoveKeyword(MyKeywords.Phantom); // 移除关键词
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, cardPlay.Card.Owner);
        }
    }


    // public override Task AfterActEntered()
    // {
    //     Log.Info("AfterActEntered");
    //     return Task.CompletedTask;
    // }

}