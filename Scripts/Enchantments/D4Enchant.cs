using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Enchantments;

// D4：附魔时，将卡牌上的所有数值重掷为1到4之间的随机值
[RegisterEnchantment]
public class D4Enchant : DieEnchant
{
    protected override int MaxFace => 4;
}
