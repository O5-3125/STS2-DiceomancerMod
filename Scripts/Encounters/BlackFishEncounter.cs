using Diceomancer.Scripts.Monsters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Encounters;

// 荣耀层boss遭遇：黑色的鱼
// [RegisterActEncounter(typeof(Glory))]
public class BlackFishEncounter : ModEncounterTemplate
{
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<BlackFish>()];

    // protected override bool HasCustomBackground => true;
    
    public override bool IsWeak => false;

    public override RoomType RoomType => RoomType.Boss;

    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() => [
        (ModelDb.Monster<BlackFish>().ToMutable(), null)
    ];
}
