using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace Diceomancer.Scripts.Common.Keywords;

internal static class BonusKeywordRegistration
{
    [RegisterOwnedCardKeyword("Bonus",
        CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.BeforeCardDescription)]
    private sealed class BonusKeyword;
}

public static class Bonus
{
    public static string BonusKeywordId =>
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, "Bonus");

    private static bool HasBonus(CardModel? card)
    {
        return card != null && card.Keywords.Contains(MyKeywords.Bonus);
    }

    private static async Task TriggerBonusEffect(PlayerChoiceContext choiceContext, CardModel card)
    {
        // 附赠
        // if (card.Keywords.Contains(MyKeywords.Bonus)) 
        await CardPileCmd.Draw(choiceContext, 1m, card.Owner);
    }

    [RegisterSingleton]
    public sealed class BonusSingleton : SingletonModel
    {
        public BonusSingleton()
        {
            ModHelper.SubscribeForCombatStateHooks(Id.Entry, CombatSubModels);
        }

        public override bool ShouldReceiveCombatHooks => true;

        private IEnumerable<AbstractModel> CombatSubModels(CombatState _)
        {
            return [this];
        }

        // 抽到的逻辑
        public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
        {
            // 附赠
            if (!HasBonus(card)) return Task.CompletedTask;

            return TriggerBonusEffect(choiceContext, card);
        }
    }
}