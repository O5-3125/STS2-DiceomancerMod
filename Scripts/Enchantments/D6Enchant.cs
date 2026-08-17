using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Enchantments;

// D6：附魔时，将卡牌上的所有数值重掷为1到6之间的随机值
[RegisterEnchantment]
public class D6Enchant : DieEnchant
{
    protected override int MaxFace => 6;
}
