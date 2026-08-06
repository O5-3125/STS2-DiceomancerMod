using Diceomancer.Scripts.Monsters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Encounters;

// 密林小怪遭遇：乌鸦（单乌鸦测试遭遇）
// 设计上还有一个5乌鸦的强怪遭遇，暂时只做单乌鸦。
// [RegisterActEncounter(typeof(Overgrowth))]
public class CrowEncounter : ModEncounterTemplate
{
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<Crow>()];

    public override bool IsWeak => true;

    public override RoomType RoomType => RoomType.Monster;

    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() => [
        (ModelDb.Monster<Crow>().ToMutable(), null)
    ];
}
