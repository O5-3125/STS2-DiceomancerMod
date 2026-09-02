using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Hero.CardPool;

// 选项牌池
[RegisterSharedCardPool]
public class OptionsCardPool : TypeListCardPoolModel
{
    // 卡池的ID。必须唯一防撞车。
    public override string Title => "抉择-选项";
    
    // 卡池是否是无色。例如事件、状态等卡池就是无色的。
    public override bool IsColorless => false;

    public override string EnergyColorName => "colorless";

    public override Color DeckEntryCardColor => new("A3A3A3FF");
}