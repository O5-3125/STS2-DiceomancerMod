using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Uncommon;

[RegisterRelic(typeof(SharedRelicPool))]
public class SixSidedDie : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    public override string PackedIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string BigIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
}
