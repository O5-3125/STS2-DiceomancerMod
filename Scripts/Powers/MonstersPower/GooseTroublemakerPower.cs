using Diceomancer.Scripts.Monsters;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.MonstersPower;

// 捣蛋鬼：鹅的特殊能力
// 每回合鹅会随机获得一种弃牌buff；弃牌buff只维持一回合，在玩家回合结束时清除。
[RegisterPower]
public class GooseTroublemakerPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/MonstersPower/prim_ability_goose.png",
        $"res://Diceomancer/images/Power/MonstersPower/prim_ability_goose.png"
    );

    // 玩家回合结束时，清除尚未触发的弃牌buff（buff只维持一回合）
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player) return;

        var buff1 = Owner.GetPower<GooseDiscardBuff1Power>();
        if (buff1 != null) await PowerCmd.Remove(buff1);

        var buff2 = Owner.GetPower<GooseDiscardBuff2Power>();
        if (buff2 != null) await PowerCmd.Remove(buff2);
    }
}
