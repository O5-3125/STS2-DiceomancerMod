using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Runs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Rare;

// [RegisterRelic(typeof(BuilderRelicPool))]
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


        var allPools = ModelDb.AllCardPools.ToArray();
        return options.WithCardPools(allPools);


        // var allCharacterPools = ModelDb.AllCharacterCardPools.ToArray();
        // return CardCreationOptions.ForNonCombatWithDefaultOdds(allCharacterPools);


        // if (!options.Flags.HasFlag(CardCreationFlags.IsCardReward))
        // {
        //     return options;
        // }
        // if (options.CardPools.All((CardPoolModel p) => p.IsColorless))
        // {
        //     return options;
        // }
        // options.
    }
}