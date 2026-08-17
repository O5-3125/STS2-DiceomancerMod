using Diceomancer.Scripts.Enchantments;
using Diceomancer.Scripts.Relics.Basic;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Relics.Ancient;

[RegisterRelic(typeof(SharedRelicPool))]
public class D20Die : DieRelic
{
    protected override string OptionKey => "ENCHANT_D20";
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override void EnchantCard(CardModel card)
    {
        CardCmd.Enchant<D20Enchant>(card, 1m);
    }
}