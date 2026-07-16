using MegaCrit.Sts2.Core.Entities.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.NormalityPower;

[RegisterPower]
public class TechPower : ModPowerTemplate
{
    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/Tech.png",
        $"res://Diceomancer/images/Power/Tech.png"
    );
    
    // 类型，Buff或Debuff
    public override PowerType Type => PowerType.Buff;

    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType => PowerStackType.Counter;
}