using Godot;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Diceomancer.Scripts.Hero.Berserker;

public class BerserkerCardPool : TypeListCardPoolModel
{
    // 如果你想用原版卡框换色，加这两行
    private static readonly Material?
        // _poolFrameMaterial = MaterialUtils.CreateReplaceHueShaderMaterial(0.87f, 0.22f, 0.46f);
        _poolFrameMaterial = MaterialUtils.CreateHsvShaderMaterial(0.94f, 1.75f, 0.87f);

    // 卡池的ID。必须唯一防撞车。
    public override string Title => "Diceomancer";
    public override string EnergyColorName => "Diceomancer";

    // 卡牌描述，遗物描述中使用的能量图标。大小为24x24。
    public override string? TextEnergyIconPath => "res://Diceomancer/images/Energy/D20.png";

    // tooltip和卡牌左上角的能量图标。大小为74x74。
    public override string? BigEnergyIconPath => "res://Diceomancer/images/Energy/D20_big.png";

    // 能量表盘文字轮廓颜色
    public override Color EnergyOutlineColor => new(0.87f, 0.22f, 0.46f);

    // 卡池的主题色。
    public override Color DeckEntryCardColor => new(0.87f, 0.22f, 0.46f);

    // 如果你是自定义卡框，上面一行换成这个
    // private static readonly Material? _poolFrameMaterial = MaterialUtils.CreateUnmodulatedHsvShaderMaterial();
    public override Material? PoolFrameMaterial => _poolFrameMaterial;

    // 卡池是否是无色。例如事件、状态等卡池就是无色的。
    public override bool IsColorless => false;
}