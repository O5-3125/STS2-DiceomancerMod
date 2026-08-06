using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.MonstersPower;

// 弃牌buff1：你打出任何牌后，弃掉你1张手牌，buff消失
[RegisterPower]
public class GooseDiscardBuff1Power : ModPowerTemplate
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/MonstersPower/prim_ability_dont_waste.png",
        $"res://Diceomancer/images/Power/MonstersPower/prim_ability_dont_waste.png"
    );

    // 玩家打出任何牌后，弃掉1张手牌
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var player = cardPlay.Player;
        if (player == null || player.Creature.IsDead) return;

        var hand = PileType.Hand.GetPile(player);
        if (hand.Cards is { Count: > 0 })
        {
            var cardModel = player.RunState.Rng.Shuffle.NextItem(hand.Cards);
            if (cardModel != null) await CardCmd.Discard(choiceContext, cardModel);
        }

        Flash();
        await PowerCmd.Remove(this);
    }
}
