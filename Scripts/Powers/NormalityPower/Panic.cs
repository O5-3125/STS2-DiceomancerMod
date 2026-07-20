using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.NormalityPower;

[RegisterPower]
public class Panic : ModPowerTemplate
{
    // 类型，Buff或Debuff
    public override PowerType Type => PowerType.Debuff;

    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/{GetType().Name}.png",
        $"res://Diceomancer/images/Power/{GetType().Name}.png"
    );

    // 每出一张牌
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(Owner.Player);

        var hand = PileType.Hand.GetPile(Owner.Player);
        if (hand.Cards is { Count: > 0 })
        {
            var cardModel = Owner.Player.RunState.Rng.Shuffle.NextItem(hand.Cards);
            if (cardModel != null) await CardCmd.Discard(choiceContext, cardModel);
        }

        Flash();
        await PowerCmd.Decrement(this);
    }

    // 回合结束消失
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == Owner.Side) await PowerCmd.Remove(this);
    }
}