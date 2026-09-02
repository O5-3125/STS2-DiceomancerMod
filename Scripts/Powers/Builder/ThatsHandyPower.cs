using Diceomancer.Scripts.Common.Keywords;
using Diceomancer.Scripts.Common.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.Builder;

[RegisterPower]
public class ThatsHandyPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/{GetType().Name}.png",
        $"res://Diceomancer/images/Power/{GetType().Name}.png"
    );
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay.Card;
        if (cardPlay.Player != Owner.Player || card.Rarity != CardRarity.Token)
            return;


        ArgumentNullException.ThrowIfNull(Owner.CombatState);
        var enemy = Owner.CombatState.RunState.Rng.CombatTargets.NextItem(Owner.CombatState.HittableEnemies);
        if (enemy == null) return;

        await CreatureCmd.Damage(choiceContext, enemy, Amount,
            ValueProp.Unpowered, null, null);
    }
}