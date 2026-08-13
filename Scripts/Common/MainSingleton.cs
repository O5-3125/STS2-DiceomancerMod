using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interactions.RightClick;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;

namespace Diceomancer.Scripts.Common;

[RegisterSingleton]
public class MainSingleton : SingletonModel
    // , IModRightClickableCard
{
    public MainSingleton()
    {
        ModHelper.SubscribeForCombatStateHooks(Id.Entry, state => [this]);
        ModHelper.SubscribeForRunStateHooks(Id.Entry, state => [this]);
    }

    public override bool ShouldReceiveCombatHooks => true;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Tags.Contains(MyTags.Evolution.GetModCardTag()))
        {
            ModifyCardCmd.ModifyCardDynamicVarsAdditive(cardPlay.Card,
                (int)cardPlay.Card.DynamicVars["Evolution"].BaseValue, true);
        }
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