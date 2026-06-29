using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers;

[RegisterPower]
public class Tulpenmanie : ModPowerTemplate
{
    // 类型，Buff或Debuff
    public override PowerType Type => PowerType.Buff;

    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType => PowerStackType.Single;

    // 自定义图标路径。1:1即可。原版游戏大图256x256，小图64x64。
    // 自定义图标路径。1:1即可。原版游戏大图256x256，小图64x64。
    public override PowerAssetProfile AssetProfile => new(
        // IconPath: $"res://Diceomancer/images/Power/{GetType().Name}.png",
        // BigIconPath: $"res://Diceomancer/images/Power/{GetType().Name}_big.png"
        "res://Diceomancer/images/Power/郁金香.png",
        "res://Diceomancer/images/Power/郁金香.png"
    );


    // 回合结束时
    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        ArgumentNullException.ThrowIfNull(Owner.Player);
        // 获取手牌
        var hand = PileType.Hand.GetPile(Owner.Player);

        Flash();
        foreach (var card in hand.Cards.ToList())
        {
            var keyList = card.DynamicVars.Keys;
            foreach (var key in keyList) card.DynamicVars[key].BaseValue = 1;
            // card.DynamicVars.Block.BaseValue = 1; // 格挡值
        }
    }


    // 回合开始后
    public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
    {
        ArgumentNullException.ThrowIfNull(Owner.Player);
        // 获取手牌
        var hand = PileType.Hand.GetPile(Owner.Player);

        Flash();
        foreach (var card in hand.Cards)
        {
            var keyList = card.DynamicVars.Keys;
            foreach (var key in keyList) card.DynamicVars[key].BaseValue *= 2;
            // card.DynamicVars.Block.BaseValue = 1; // 格挡值
        }
    }
}