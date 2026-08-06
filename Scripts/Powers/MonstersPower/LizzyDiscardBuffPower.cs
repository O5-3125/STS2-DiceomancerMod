using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.MonstersPower;

// 可爱的大手弃牌buff：当玩家打出10张牌后，丢弃玩家一半手牌（向下取整），buff消失
[RegisterPower]
public class LizzyDiscardBuffPower : ModPowerTemplate
{
    private const int TriggerCount = 10;

    private int _cardsPlayed;

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/MonstersPower/prim_ability_dont_waste.png",
        $"res://Diceomancer/images/Power/MonstersPower/prim_ability_dont_waste.png"
    );

    // 玩家打出牌时计数
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        _cardsPlayed++;
        if (_cardsPlayed < TriggerCount) return;

        var player = cardPlay.Player;
        var hand = PileType.Hand.GetPile(player);

        // 丢弃一半手牌（向下取整）
        var discardCount = hand.Cards.Count / 2;
        var pool = hand.Cards.ToList();
        var toDiscard = new List<CardModel>();

        for (var i = 0; i < discardCount && pool.Count > 0; i++)
        {
            var idx = player.RunState.Rng.Shuffle.NextInt(0, pool.Count);
            toDiscard.Add(pool[idx]);
            pool.RemoveAt(idx);
        }

        if (toDiscard.Count > 0)
        {
            await CardCmd.Discard(choiceContext, toDiscard);
        }

        Flash();
        await PowerCmd.Remove(this);
    }
}
