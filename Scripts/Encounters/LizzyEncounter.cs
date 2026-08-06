using Diceomancer.Scripts.Monsters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Encounters;

// 虫巢2层精英遭遇：可爱的大手
// [RegisterActEncounter(typeof(Hive))]
public class LizzyEncounter : ModEncounterTemplate
{
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<Lizzy>()];

    public override bool IsWeak => false;

    public override RoomType RoomType => RoomType.Elite;

    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() => [
        (ModelDb.Monster<Lizzy>().ToMutable(), null)
    ];
}
