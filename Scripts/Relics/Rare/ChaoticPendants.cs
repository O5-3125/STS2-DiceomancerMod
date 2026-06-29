using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Rare;

// 加入角色遗物池
// [RegisterRelic(typeof(DiceomancerRelicPool))]
//TODO 混沌吊坠
public class ChaoticPendants : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    // private CardGeneratedEntry _cardGeneratedEntry;


    // 修改战斗掉落
    public override CardCreationOptions ModifyCardRewardCreationOptions(Player player, CardCreationOptions options)
    {
        if (Owner != player) return options;

        if (options.Flags.HasFlag(CardCreationFlags.NoCardPoolModifications)) return options;

        if (!options.Flags.HasFlag(CardCreationFlags.IsCardReward)) return options;

        if (options.CustomCardPool != null) return options;

        var pools = player.UnlockState.CardPools.Union(options.CardPools);
        return options.WithCardPools(pools, options.CardPoolFilter);
    }

    // 修改商店卡创建结果
    public override void ModifyMerchantCardCreationResults(Player player, List<CardCreationResult> cards)
    {
        // cards.


        base.ModifyMerchantCardCreationResults(player, cards);
    }

    // 修改商店卡池
    public override IEnumerable<CardModel> ModifyMerchantCardPool(Player player, IEnumerable<CardModel> options)
    {
        return base.ModifyMerchantCardPool(player, options);
    }


    // await CardCmd.Exhaust(new ThrowingPlayerChoiceContext(), card);
    // await CardCmd.TransformToRandom(new ThrowingPlayerChoiceContext(), card);
    // await CardCmd.ClearEnchantment(new ThrowingPlayerChoiceContext(), card);
    // await CardCmd.Afflict(new ThrowingPlayerChoiceContext(), card);
    // }


    // 小图标（原版85x85）
    // public override string PackedIconPath => $"res://Diceomancer/images/Relics/BuilderRing.png";
    // // 轮廓图标（原版85x85）
    // protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/BuilderRing.png";
    // // 大图标（原版256x256）
    // protected override string BigIconPath => $"res://Diceomancer/images/Relics/BuilderRing_big.png";
}