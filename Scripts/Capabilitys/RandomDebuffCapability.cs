using Diceomancer.Scripts.Common.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;

namespace Diceomancer.Scripts.Capabilitys;

[RegisterModelCapability]
public class RandomDebuffCapability : CardPlayCapability, ICardDescriptionContributor
{
    protected override void OnAttach(CardModel model)
    {
        Log.Info("组件被挂载");
    }

    protected override void OnDetach(CardModel model)
    {
        Log.Info("组件被卸载");
    }

    public IEnumerable<CardDescriptionFragment> GetDescriptionFragments(CardDescriptionContext context) =>
        [new(new LocString("enchantments", $"{Id.Entry}.description"))];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new("Debuff", 1)];

    protected override async Task OnOwnerCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay.Card;

        ArgumentNullException.ThrowIfNull(card.CombatState);
        
        var enemy = card.Owner.RunState.Rng.CombatTargets.NextItem(card.CombatState.HittableEnemies);

        if (enemy != null)
            await DiceomancerCardCmd.ApplyRandomDebuff(choiceContext, card.Owner,
                enemy, card.Owner.Creature, card, DynamicVars["Debuff"].IntValue);
    }
}