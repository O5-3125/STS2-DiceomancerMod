using Diceomancer.Scripts.Relics.Ancient;
using Diceomancer.Scripts.Relics.Rare;
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

// [RegisterSharedAncient] // 如果需要自定义生成条件，可以注册成通用再重载isAllowed
[RegisterActAncient(typeof(Glory))] // 指定只有荣耀这章生成
public class TestAncient : ModAncientEventTemplate
{
    public override Color ButtonColor => new(0.533f, 0.247f, 0.961f);

    public override Color DialogueColor => new(0.533f, 0.247f, 0.961f);

    public override EventAssetProfile AssetProfile => new(
        BackgroundScenePath: "res://Diceomancer/scenes/Ancients/test_ancient.tscn"
    );

    public override AncientEventPresentationAssetProfile AncientPresentationAssetProfile => new(
        MapIconPath: "res://Diceomancer/images/Ancient/dice.png", // 地图标志
        MapIconOutlinePath: "res://Diceomancer/images/Ancient/dice.png",
        RunHistoryIconPath: "res://Diceomancer/images/Ancient/check_3.png", // 对话头像
        RunHistoryIconOutlinePath: "res://Diceomancer/images/Ancient/check_3.png"
    );

    private IReadOnlyList<EventOption> Pool1 =>
    [
        CreateModRelicOption<FoulPlay>(),
        CreateModRelicOption<HeartOfSteel>(),
        CreateModRelicOption<D20Die>(),
    ];

    private IReadOnlyList<EventOption> Pool2 =>
    [
        CreateModRelicOption<PiggyToy>(),
        CreateModRelicOption<Status>(),
    ];

    private IReadOnlyList<EventOption> Pool3 =>
    [
        CreateModRelicOption<InspirationVoid>(),
        CreateModRelicOption<CardBook>(),
        CreateModRelicOption<VialOfForgetfulness>(),
    ];

    // 所有可能的选项
    public override IEnumerable<EventOption> AllPossibleOptions => [.. Pool1, .. Pool2, .. Pool3];

    // 生成选项
    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            Rng.NextItem(Pool1)!,
            Rng.NextItem(Pool2)!,
            Rng.NextItem(Pool3)!,
        ];
    }
}