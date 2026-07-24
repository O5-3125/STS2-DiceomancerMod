using Diceomancer.Scripts.Monsters;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Error = Diceomancer.Scripts.Monsters.Error;

namespace Diceomancer.Scripts.Encounters;

// [RegisterActEncounter(typeof(Hive))]
public class TestEncounter : ModEncounterTemplate
{
    public override bool IsWeak => true; // 这个遭遇是否是弱怪池

    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<Error>()]; // 所有可能出现的怪物

    public override RoomType RoomType => RoomType.Monster; // 这个遭遇的房间类型，这里是普通怪物

    // 不要忘了这里的model需要调用ToMutable()，表示不是标准值而是战斗中的可变数据
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
    {
        // 如果不想指定怪物生成在哪个槽位，可以直接传null，系统会自动分配
        return [(ModelDb.Monster<Error>().ToMutable(), null)];
    }
    
    // public override EncounterAssetProfile AssetProfile => new(
    //     EncounterScenePath: "res://Diceomancer/scenes/Encounters/TestEncounter.tscn",
    //     BackgroundScenePath: "res://Diceomancer/scenes/Encounters/TestEncounter.tscn",
    //     BackgroundLayersDirectoryPath: "res://scenes/backgrounds/diceomancer_encounter_test_encounter/TestEncounter_bg_.tscn",
    //     RunHistoryIconPath: "",
    //     RunHistoryIconOutlinePath: ""
    // );
    
    // public override string BossNodePath => AssetProfile.BossNodeSpinePath;
    // public override string? CustomBackgroundScenePath => "";
    // public override string? CustomBackgroundLayersDirectoryPath => "";
    // public override string? CustomEncounterScenePath => "";
    // public override string? CustomBossNodePath => "";
    // public override string? CustomRunHistoryIconPath => "";
    // public override string? CustomRunHistoryIconOutlinePath => "";
    // public override IEnumerable<string> ExtraAssetPaths => [""];
    // public override IEnumerable<string>? CustomExtraAssetPaths => [""];
    // public override IEnumerable<string>? CustomMapNodeAssetPaths => [""];
    //
    // public override bool HasScene => true;
    // protected override bool HasCustomBackground => true;
    // protected override bool UseActCombatBackground => false;
    // protected override bool UseProgrammaticCombatBackground => false;
}