using Diceomancer.Scripts.Monsters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Encounters;

// 密林精英遭遇：神选草
// [RegisterActEncounter(typeof(Overgrowth))]
public class GrassGuyEncounter : ModEncounterTemplate
{
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<GrassGuy>()];

    public override bool IsWeak => false;

    public override RoomType RoomType => RoomType.Elite;

    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() => [
        (ModelDb.Monster<GrassGuy>().ToMutable(), null)
    ];
}
