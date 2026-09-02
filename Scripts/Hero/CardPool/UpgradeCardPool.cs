using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Diceomancer.Scripts.Hero.CardPool;

[RegisterSharedCardPool]
public class UpgradeCardPool : TypeListCardPoolModel
{
    // 如果你想用原版卡框换色，加这两行
    private static readonly Material?
        _poolFrameMaterial = MaterialUtils.CreateReplaceHueShaderMaterial(0.5f, 0.5f, 0.5f);

    // 卡池的ID。必须唯一防撞车。
    public override string Title => "建造完成";

    public override string EnergyColorName => "colorless";

    public override Color DeckEntryCardColor => new("A3A3A3FF");


    // 如果你是自定义卡框，上面一行换成这个
    // private static readonly Material? _poolFrameMaterial = MaterialUtils.CreateUnmodulatedHsvShaderMaterial();
    public override Material? PoolFrameMaterial => _poolFrameMaterial;

    // 卡池是否是无色。例如事件、状态等卡池就是无色的。
    public override bool IsColorless => false;
}