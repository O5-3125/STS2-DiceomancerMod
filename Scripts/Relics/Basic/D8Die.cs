using Diceomancer.Scripts.Enchantments;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Relics.Basic;

[RegisterRelic(typeof(SharedRelicPool))]
public class D8Die : DieRelic
{
    protected override string OptionKey => "ENCHANT_D8";
    public override RelicRarity Rarity => RelicRarity.Starter;

    protected override void EnchantCard(CardModel card)
    {
        CardCmd.Enchant<D8Enchant>(card, 1m);
    }
}
