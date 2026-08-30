using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Enchantments;

// D20：附魔时，将卡牌上的所有数值重掷为1到20之间的随机值
[RegisterEnchantment]
public class D20Enchant : DieEnchant
{
    protected override int MaxFace => 20;
}
