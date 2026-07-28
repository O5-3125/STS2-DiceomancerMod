using Diceomancer.Scripts.Relics.Ancient;
using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Relics;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Diceomancer.Scripts.Ancients;

[RegisterSharedAncient] // 如果需要自定义生成条件，可以注册成通用再重载isAllowed
public class TestAncient : ModAncientEventTemplate
{
    // 选项按钮颜色
    public override Color ButtonColor => new(0.533f, 0.247f, 0.961f);

    // 对话框颜色
    public override Color DialogueColor => new(0.533f, 0.247f, 0.961f);

    // 自定义场景的路径
    public override EventAssetProfile AssetProfile => new(
        BackgroundScenePath: "res://Diceomancer/scenes/Ancients/test_ancient.tscn"
    );

    // 对于图片，只要是godot支持的格式都可以，例如png,jpg,svg等等
    // 自定义地图图标和轮廓的路径
    public override AncientEventPresentationAssetProfile AncientPresentationAssetProfile => new(
        MapIconPath: "res://Diceomancer/images/Ancient/dice.png",// 地图标志
        MapIconOutlinePath: "res://Diceomancer/images/Ancient/dice.png",
        RunHistoryIconPath: "res://Diceomancer/images/Ancient/check_3.png",// 对话头像
        RunHistoryIconOutlinePath: "res://Diceomancer/images/Ancient/check_3.png"
    );

    // 固定池一和二
    private IReadOnlyList<EventOption> Pool1 =>
    [
        CreateModRelicOption<Freedom>(),
        CreateModRelicOption<HeartOfSteel>(),
    ];

    private IReadOnlyList<EventOption> Pool2 =>
    [
        CreateModRelicOption<InspirationVoid>(),
        CreateModRelicOption<Ouroboros>(),
    ];

    // 带权重池三。权重越大越有机会生成。当然你也可以写自定义的列表生成函数
    private WeightedList<EventOption> Pool3 => new()
    {
        { CreateModRelicOption<Status>(), 2 },
        { CreateModRelicOption<TheStabilizer>(), 1 }
    };

    // 所有可能的选项
    public override IEnumerable<EventOption> AllPossibleOptions => [.. Pool1, .. Pool2, .. Pool3];

    // 生成选项
    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            Rng.NextItem(Pool1)!,
            Rng.NextItem(Pool2)!,
            Pool3.GetRandom(Rng),
        ];
    }


    // 出现条件。这里是只能在密林出现
    // public override bool IsValidForAct(ActModel act) {
    //     return act is Overgrowth;
    // }
}