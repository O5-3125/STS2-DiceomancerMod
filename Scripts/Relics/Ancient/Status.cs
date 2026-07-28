using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Ancient;

[RegisterRelic(typeof(EventRelicPool))]
public class Status : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override CardCreationOptions ModifyCardRewardCreationOptions(Player player, CardCreationOptions options)
    {
        if (base.Owner != player)
        {
            return options;
        }

        if (options.Flags.HasFlag(CardCreationFlags.NoCardPoolModifications))
        {
            return options;
        }

        if (!options.Flags.HasFlag(CardCreationFlags.IsCardReward))
        {
            return options;
        }
        // if (options.CardPools.All((CardPoolModel p) => p.IsColorless))
        // {
        //     return options;
        // }

        return options.WithRarityOdds(CardRarityOddsType.BossEncounter);
    }

    public override CardRarity ModifyMerchantCardRarity(Player player, CardRarity rarity)
    {
        return base.Owner != player ? rarity : CardRarity.Rare;
    }
}