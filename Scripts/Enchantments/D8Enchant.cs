using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Enchantments;

// D8：附魔时，将卡牌上的所有数值重掷为1到8之间的随机值
[RegisterEnchantment]
public class D8Enchant : DieEnchant
{
    protected override int MaxFace => 8;
}
