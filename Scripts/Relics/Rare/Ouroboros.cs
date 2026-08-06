using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Rare;

[RegisterRelic(typeof(SharedRelicPool))]
public class Ouroboros : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    public override string PackedIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string BigIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";

    public override bool ShouldTakeExtraTurn(Player player)
    {
        Flash();
        return (Owner.GetEnergy() == 0) // 当前费用为0
               & PileType.Hand.GetPile(Owner).IsEmpty // 手牌是空的
               & (player == Owner); // 玩家拥有遗物
    }
}