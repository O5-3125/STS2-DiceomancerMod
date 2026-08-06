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

// 火猫三丈被动：玩家回合开始时，手牌中的数字随机变化
// 变化规则：将数字N拆解为若干个骰子（D20/D12/D8/D6/D4，面数20/12/8/6/4），掷出各骰子和后相加得出变化后的数。
// N=0或1 -> 单个D4；N<0按0处理；N>1 -> 将2N拆为面数集合{20,12,8,6,4}的和，骰子数尽可能少，且优先使用大骰子更多的组合。
[RegisterPower]
public class CatNumberChaosPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/MonstersPower/cat.png",
        $"res://Diceomancer/images/Power/MonstersPower/cat.png"
    );
    
    // 玩家回合开始时，变化手牌上的所有数字
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Flash();
        var hand = PileType.Hand.GetPile(player).Cards.ToList();

        ModifyCardCmd.DiceRollCardList(player, hand);
    }
}