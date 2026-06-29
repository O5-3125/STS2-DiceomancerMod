using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using STS2RitsuLib.Combat.HandSize;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers;

[RegisterPower]
public class Colossus : ModPowerTemplate, IMaxHandSizeModifier
{
    public override PowerType Type => PowerType.Debuff;

    // public override PowerStackType StackType => PowerStackType.Counter;
    public override PowerStackType StackType => PowerStackType.Single;


    // 修改手牌上限
    public int ModifyMaxHandSizeLate(Player player, int currentMaxHandSize)
    {
        return player != Owner.Player ? currentMaxHandSize : Amount;
    }
}