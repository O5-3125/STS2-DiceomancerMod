using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using STS2RitsuLib.Cards.Transforms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers;

[RegisterPower]
public class MassProductionPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/{GetType().Name}.png",
        $"res://Diceomancer/images/Power/{GetType().Name}.png"
    );


    
}
