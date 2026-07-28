using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interactions.RightClick;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Common;

[RegisterSingleton]
public class MainSingleton : SingletonModel, IModRightClickableCard
{
    public MainSingleton()
    {
        ModHelper.SubscribeForCombatStateHooks(Id.Entry, state => [this]);
        ModHelper.SubscribeForRunStateHooks(Id.Entry, state => [this]);
    }

    public override bool ShouldReceiveCombatHooks => true;

    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Tags.Contains(MyTags.Evolution.GetModCardTag()))
        {
            var keyList = cardPlay.Card.DynamicVars.Keys;
            foreach (var key in keyList)
                if (key != "Evolution")
                    cardPlay.Card.DynamicVars[key].BaseValue += cardPlay.Card.DynamicVars["Evolution"].BaseValue;
        }
    }

    public async Task OnRightClick(ModRightClickExecutionContext context)
    {
        // if (context.PlayerChoiceContext == null) return;
        //
        // var card = (CardModel)context.Model;
        //
        // if (card.Keywords.Contains(MyKeywords.Diabolical))
        // {
        //     await CardCmd.Exhaust(context.PlayerChoiceContext, card);
        //     await CardPileCmd.Draw(context.PlayerChoiceContext, 1m, card.Owner);
        //
        //     // await SecondaryResourceCmd.Gain(context.Player, BlackMana.ManaId, 1);
        //     // await PowerCmd.Apply<DoomPower>(context.PlayerChoiceContext, context.Player.Creature, 3, null, null);
        // }
    }


    // public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay, ResourceInfo resources,
    //     CardLocation cardLocation)
    // {
    //     if (card.Keywords.Contains(MyKeywords.Rebound) && cardLocation.pileType == PileType.Discard  )
    //     {
    //         cardLocation.pileType = PileType.Hand;
    //     }
    //
    //     return cardLocation;
    // }
}