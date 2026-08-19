using Diceomancer.Scripts.Common.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Diceomancer.Scripts.Common.Keywords;

internal static class StormKeywordRegistration
{
    [RegisterOwnedCardKeyword("Storm",
        CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
    private sealed class StormKeyword;
}

public static class Storm
{
    public static string StormKeywordId =>
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, "Storm");

    private static bool HasStorm(CardModel? card)
    {
        return card != null && card.Keywords.Contains(MyKeywords.Storm);
    }


    [RegisterSingleton]
    public sealed class StormSingleton : SingletonModel
    {
        public StormSingleton()
        {
            ModHelper.SubscribeForCombatStateHooks(Id.Entry, CombatSubModels);
        }

        public override bool ShouldReceiveCombatHooks => true;

        private IEnumerable<AbstractModel> CombatSubModels(CombatState _)
        {
            return [this];
        }

        public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
        {
            if (!HasStorm(card)) return playCount;


            var orbCount = BarbarianCardUtils.CountEmotionOrbs(card.Owner);

            return playCount + orbCount;
        }
    }
}