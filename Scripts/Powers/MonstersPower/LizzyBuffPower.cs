using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Commands;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.MonstersPower;

// 可爱的大手标记能力：每回合开始如果没有弃牌buff，获得弃牌buff
[RegisterPower]
public class LizzyBuffPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/MonstersPower/prim_ability_thats_handy.png",
        $"res://Diceomancer/images/Power/MonstersPower/prim_ability_thats_handy.png"
    );

    // 每回合（玩家回合开始）检查一次
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (!Owner.HasPower<LizzyDiscardBuffPower>())
        {
            await PowerCmd.Apply<LizzyDiscardBuffPower>(choiceContext, Owner, 1m, Owner, null);
        }
    }
}
