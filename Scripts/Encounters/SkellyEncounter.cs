using Diceomancer.Scripts.Monsters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Encounters;

// 密林小怪遭遇：骷髅哥
[RegisterActEncounter(typeof(Overgrowth))]
public class SkellyEncounter : ModEncounterTemplate
{
    // 所有可能出现的怪物
    public override IEnumerable<MonsterModel> AllPossibleMonsters => [ModelDb.Monster<Skelly>()];

    // 这个遭遇是否是弱怪池，骷髅哥是密林小怪
    public override bool IsWeak => false;

    public override RoomType RoomType => RoomType.Monster; // 这个遭遇的房间类型，这里是普通怪物

    // 不要忘了这里的model需要调用ToMutable()，表示不是标准值而是战斗中的可变数据
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters() => [
        (ModelDb.Monster<Skelly>().ToMutable(), null) // 如果不想指定怪物生成在哪个槽位，可以直接传null，系统会自动分配
    ];
}
