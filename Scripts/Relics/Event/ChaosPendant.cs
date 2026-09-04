using Diceomancer.Scripts.Hero.Builder;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Event;

[RegisterRelic(typeof(EventRelicPool))]
public class ChaosPendant : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    public override string PackedIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string BigIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";

    public override CardCreationOptions ModifyCardRewardCreationOptions(Player player, CardCreationOptions options)
    {
        if (Owner != player) return options;
        if (options.Flags.HasFlag(CardCreationFlags.NoCardPoolModifications)) return options;
        if (!options.Flags.HasFlag(CardCreationFlags.IsCardReward)) return options;

  
        var allPools = ModelDb.AllCardPools.ToList();

        return options.WithCardPools(allPools).WithRarityOdds(CardRarityOddsType.Uniform);
    }

    public override IEnumerable<CardModel> ModifyMerchantCardPool(Player player, IEnumerable<CardModel> options)
    {
        if (Owner != player) return options;


        return options.Concat(ModelDb.AllCards).Distinct();
    }

    public override CardRarity ModifyMerchantCardRarity(Player player, CardRarity rarity)
    {
        if (Owner != player) return rarity;

        var cardRarity = Owner.RunState.Rng.Niche.NextItem(Rarities);

        return cardRarity;
    }

    private static readonly List<CardRarity> Rarities =
    [
        CardRarity.Ancient,
        // CardRarity.Basic,
        CardRarity.Event,
        CardRarity.Token,
        
        CardRarity.Common,
        CardRarity.Uncommon,
        CardRarity.Rare,
    ];
}