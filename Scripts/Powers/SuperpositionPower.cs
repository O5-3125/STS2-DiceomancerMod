using Diceomancer.Scripts.Common;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers;

[RegisterPower]
public class SuperpositionPower : ModPowerTemplate
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

    public override Task AfterModifyingCardPlayCount(CardModel card)
    {
        if (card.Keywords.Contains(MyKeywords.Chaos))
        {
            card.BaseReplayCount += 1;
        }

        return base.AfterModifyingCardPlayCount(card);
    }
}