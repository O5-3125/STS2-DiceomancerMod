using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Enchantments;

// D12：附魔时，将卡牌上的所有数值重掷为1到12之间的随机值
[RegisterEnchantment]
public class D12Enchant : DieEnchant
{
    protected override int MaxFace => 12;
}
