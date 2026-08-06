using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.MonstersPower;

// 神选草被动：自己的格挡被击破时，对玩家造成12点伤害
// 同一回合可以多次触发；没有格挡时不触发（格挡从>0被打到0才视为击破）。
[RegisterPower]
public class GrassRetaliationPower : ModPowerTemplate
{
    // 反击伤害
    private const int RetaliationDamage = 12;

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/MonstersPower/prim_ability_herb_healing.png",
        $"res://Diceomancer/images/Power/MonstersPower/prim_ability_herb_healing.png"
    );

    // 格挡被击破时触发
    public override async Task AfterBlockBroken(PlayerChoiceContext choiceContext, Creature target, Creature? breaker)
    {
        if (target != Owner) return;

        // 对击破格挡的玩家造成伤害；如果没有明确来源则对第一位玩家
        var player = breaker?.Player ?? Owner.CombatState.RunState.Players.FirstOrDefault();
        if (player == null || player.Creature.IsDead) return;

        Flash();
        await CreatureCmd.Damage(choiceContext, player.Creature, RetaliationDamage,
            ValueProp.Unpowered | ValueProp.Move, null, null);
    }
}
