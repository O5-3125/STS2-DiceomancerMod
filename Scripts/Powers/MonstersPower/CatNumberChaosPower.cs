using Diceomancer.Scripts.Common.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.MonstersPower;

[RegisterPower]
public class CatNumberChaosPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/MonstersPower/{GetType().Name}.png",
        $"res://Diceomancer/images/Power/MonstersPower/{GetType().Name}.png"
    );

    // 玩家回合开始时，变化手牌上的所有数字
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Flash();
        var hand = PileType.Hand.GetPile(player).Cards.ToList();

        ModifyCardCmd.DiceRollCardList(player, hand);
    }
}