using Diceomancer.Scripts.Common;
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
        if (card.Keywords.Contains(MyKeywords.Bonus)) await CardPileCmd.Draw(choiceContext, 1m, card.Owner);
    }


    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
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
    }


    // public override Task AfterActEntered()
    // {
    //     Log.Info("AfterActEntered");
    //     return Task.CompletedTask;
    // }

    // public async override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    // {
    //     Log.Info($"AfterCardDrawn: {card.Id}");
    // }
}