using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Combat.HandSize;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers;

[RegisterPower]
public class MaxHandSize : ModPowerTemplate, IMaxHandSizeModifier
{
    public override PowerType Type => PowerType.None;

    public override PowerStackType StackType => PowerStackType.Single;
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/{GetType().Name}.png",
        $"res://Diceomancer/images/Power/{GetType().Name}.png"
    );

    public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        // if (target.GetPower<MaxHandSize>() != null)
        // {
        await PowerCmd.Remove<MaxHandSize>(target);
        // }
    }

    // 修改手牌上限
    public int ModifyMaxHandSizeLate(Player player, int currentMaxHandSize)
    {
        return player != Owner.Player ? currentMaxHandSize : Amount;
    }
}