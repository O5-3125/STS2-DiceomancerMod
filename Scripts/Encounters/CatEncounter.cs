using Diceomancer.Scripts.Monsters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Encounters;

// 荣耀小怪遭遇：火猫三丈
[RegisterActEncounter(typeof(Glory))]
public class CatEncounter : ModEncounterTemplate
{
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<Cat>()];

    public override bool IsWeak => false;

    public override RoomType RoomType => RoomType.Monster;

    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() => [
        (ModelDb.Monster<Cat>().ToMutable(), null)
    ];
}
