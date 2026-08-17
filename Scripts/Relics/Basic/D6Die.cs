using Diceomancer.Scripts.Enchantments;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Relics.Basic;

// 加入角色遗物池
[RegisterRelic(typeof(BuilderRelicPool))]
// 加入初始遗物池
[RegisterCharacterStarterRelic(typeof(Builder))]
public class D6Die : DieRelic
{
    protected override string OptionKey => "ENCHANT_D6";
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override void EnchantCard(CardModel card)
    {
        CardCmd.Enchant<D6Enchant>(card, 1m);
    }
}
