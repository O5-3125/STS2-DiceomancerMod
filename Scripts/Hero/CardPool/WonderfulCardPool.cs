using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Hero.CardPool;

// 奇妙牌池
[RegisterSharedCardPool]
public class WonderfulCardPool : TypeListCardPoolModel
{
    // 卡池的ID。必须唯一防撞车。
    public override string Title => "Wonderful";

    public override string EnergyColorName => "Wonderful";

    // 卡池是否是无色。例如事件、状态等卡池就是无色的。
    public override bool IsColorless => true;

    // 卡池的主题色。
    public override Color DeckEntryCardColor => new(0, 0, 0);
}