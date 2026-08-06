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
    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerAssetProfile AssetProfile => new(
        IconPath: $"res://Diceomancer/images/Power/{GetType().Name}.png",
        BigIconPath: $"res://Diceomancer/images/Power/{GetType().Name}_big.png"
    );

    public override Task AfterModifyingCardPlayCount(CardModel card)
    {
        if (card.Keywords.Contains(MyKeywords.Chaos4) ||
            card.Keywords.Contains(MyKeywords.Chaos6) ||
            card.Keywords.Contains(MyKeywords.Chaos8) ||
            card.Keywords.Contains(MyKeywords.Chaos12) ||
            card.Keywords.Contains(MyKeywords.Chaos20))
        {
            card.BaseReplayCount += Amount;
        }

        return base.AfterModifyingCardPlayCount(card);
    }
}