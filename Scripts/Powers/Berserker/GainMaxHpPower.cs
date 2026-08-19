using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.Berserker;

[RegisterPower]
public class GainMaxHpPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/{GetType().Name}.png",
        $"res://Diceomancer/images/Power/{GetType().Name}.png"
    );

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        if (Amount > 0)
        {
            await CreatureCmd.GainMaxHp(Owner, Amount);
        }
    }

    // public override async Task AfterRemoved(Creature oldOwner)
    // {
    //     if (Amount > 0)
    //     {
    //         await CreatureCmd.GainMaxHp(oldOwner, Amount);
    //     }
    // }
}