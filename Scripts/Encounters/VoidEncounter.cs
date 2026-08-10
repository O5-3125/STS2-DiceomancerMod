using Diceomancer.Scripts.Monsters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Encounters;

// 荣耀boss遭遇：空图层
// [RegisterActEncounter(typeof(Overgrowth))]
public class VoidEncounter : ModEncounterTemplate
{
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<Monsters.Void>()];

    public override bool IsWeak => false;

    public override RoomType RoomType => RoomType.Boss;

    // 遭遇场景（用来指定每个怪物站哪）
    public override EncounterAssetProfile AssetProfile => new(
        EncounterScenePath: "res://Diceomancer/scenes/Encounters/TestEncounter.tscn"
    );

    // 怪物槽位的名字
    public override IReadOnlyList<string> Slots =>
    [
        "main", "left1", "left2", "right1", "right2","up"
    ];


    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() =>
    [
        (ModelDb.Monster<Monsters.Void>().ToMutable(), "main")
    ];
}