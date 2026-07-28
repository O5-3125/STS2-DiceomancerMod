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

internal static class ReboundKeywordRegistration
{
    [RegisterOwnedCardKeyword("Rebound",
        CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
    private sealed class ReboundKeyword;
}

public static class Rebound
{
    public static string ReboundKeywordId =>
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, "Rebound");

    private static bool HasRebound(CardModel? card)
    {
        return card != null && card.Keywords.Contains(MyKeywords.Rebound);
    }


    [RegisterSingleton]
    public sealed class ReboundSingleton : SingletonModel
    {
        public ReboundSingleton()
        {
            ModHelper.SubscribeForCombatStateHooks(Id.Entry, CombatSubModels);
        }

        public override bool ShouldReceiveCombatHooks => true;

        private IEnumerable<AbstractModel> CombatSubModels(CombatState _)
        {
            return [this];
        }

        public override CardLocation ModifyCardPlayResultLocation(CardModel card, bool isAutoPlay,
            ResourceInfo resources,
            CardLocation cardLocation)
        {
            if (HasRebound(card) && cardLocation.pileType == PileType.Discard)
            {
                cardLocation.pileType = PileType.Hand;
            }

            return cardLocation;
        }
    }
}