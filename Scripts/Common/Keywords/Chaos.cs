using Diceomancer.Scripts.Common.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;
using STS2RitsuLib.Models.Capabilities;

namespace Diceomancer.Scripts.Common.Keywords;

internal static class ChaosKeywordRegistration
{
    [RegisterOwnedCardKeyword("Chaos",
        CardDescriptionPlacement = ModKeywordCardDescriptionPlacement.AfterCardDescription)]
    private sealed class ChaosKeyword;
}

public static class Chaos
{
    public static string ChaosKeywordId =>
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, "Chaos");

    private static bool HasChaos(CardModel? card)
    {
        return card != null && card.Keywords.Contains(MyKeywords.Chaos);
    }

    private static async Task TriggerChaosEffect(PlayerChoiceContext choiceContext, CardModel card)
    {
        var keyList = card.DynamicVars.Keys.ToList();
        foreach (var key in keyList)
            card.DynamicVars[key].BaseValue = RandomCmd.CheckD6(card.Owner);


        if (card.Enchantment != null)
        {
            var eKeyList = card.Enchantment.DynamicVars.Keys.ToList();
            foreach (var eKey in eKeyList)
                card.Enchantment.DynamicVars[eKey].BaseValue = RandomCmd.CheckD6(card.Owner);

            // var capabilities = card.Capabilities().All.ToList();
            // foreach (var capability in capabilities)
            // {
            //     // capability.CapabilityId
            // }
        }
    }

    [RegisterSingleton]
    public sealed class ChaosSingleton : SingletonModel
    {
        public ChaosSingleton()
        {
            ModHelper.SubscribeForCombatStateHooks(Id.Entry, CombatSubModels);
        }

        public override bool ShouldReceiveCombatHooks => true;

        private IEnumerable<AbstractModel> CombatSubModels(CombatState _)
        {
            return [this];
        }

        public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
        {
            if (!HasChaos(card)) return Task.CompletedTask;
            return TriggerChaosEffect(choiceContext, card);
        }
    }
}