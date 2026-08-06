using Diceomancer.Scripts.Monsters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Encounters;

// 虫巢2层怪物遭遇：大红帽子怪
[RegisterActEncounter(typeof(Hive))]
public class HatEncounter : ModEncounterTemplate
{
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<Hat>()];

    public override bool IsWeak => false;

    public override RoomType RoomType => RoomType.Monster;

    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() => [
        (ModelDb.Monster<Hat>().ToMutable(), null)
    ];
}
