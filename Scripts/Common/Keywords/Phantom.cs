using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Diceomancer.Scripts.Common.Keywords;

internal static class PhantomKeywordRegistration
{
    [RegisterOwnedCardKeyword("Phantom",
        CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
    private sealed class PhantomKeyword;
}

public static class Phantom
{
    public static string PhantomKeywordId =>
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, "Phantom");

    private static bool HasPhantom(CardModel? card)
    {
        return card != null && card.Keywords.Contains(MyKeywords.Phantom);
    }

    private static async Task TriggerPhantomEffect(PlayerChoiceContext choiceContext, CardModel card)
    {
        var cardModel = card.CreateClone();
        cardModel.EnergyCost.AddThisCombat(-1);
        cardModel.AddKeyword(CardKeyword.Exhaust);
        cardModel.RemoveKeyword(MyKeywords.Phantom);
        await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, card.Owner);
    }

    [RegisterSingleton]
    public sealed class PhantomSingleton : SingletonModel
    {
        public PhantomSingleton()
        {
            ModHelper.SubscribeForCombatStateHooks(Id.Entry, CombatSubModels);
        }

        public override bool ShouldReceiveCombatHooks => true;

        private IEnumerable<AbstractModel> CombatSubModels(CombatState _)
        {
            return [this];
        }

        public override Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (!HasPhantom(cardPlay.Card)) return Task.CompletedTask;
            return TriggerPhantomEffect(choiceContext, cardPlay.Card);
        }
    }
}