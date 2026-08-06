using Diceomancer.Scripts.Cards.Curse;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Runs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Events;

[RegisterActEvent(typeof(Glory))]
public sealed class Exchange : ModEventTemplate
{
    // 背景图位置,目前没有背景图所以暂时用TestEvent.png代替
    public override EventAssetProfile AssetProfile => new(
        InitialPortraitPath: "res://Diceomancer/images/Event/TestEvent.png"
    );

    // 设置一些数值
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new GoldVar(60),
        new GoldVar("WinGold", 100),
        new GoldVar("FailGold", 1),
        new StringVar("VertigoCard", ModelDb.Card<Vertigo>().Title)
    ];

    // 至少要付得起交易费
    public override bool IsAllowed(IRunState runState)
    {
        return runState.Players.All(p => p.Gold >= DynamicVars.Gold.BaseValue);
    }

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            new EventOption(this, Trade, InitialOptionKey("TRADE")),
            new EventOption(this, Leave, InitialOptionKey("LEAVE"), HoverTipFactory.FromCard<Vertigo>())
        ];
    }

    // 交易一轮试试！失去60金币，50%概率获得100金币，否则获得1金币
    private async Task Trade()
    {
        await PlayerCmd.LoseGold(DynamicVars.Gold.BaseValue, Owner!, GoldLossType.Stolen);
        await RollTrade(Owner.RunState.Rng.Niche.NextBool());
        SetEventState(L10NLookup($"{Id.Entry}.pages.CONTINUE.description"), ContinueOptions());
    }

    // 继续交易！！失去60金币，40%概率获得100金币，否则获得1金币，然后重复本场景
    private async Task TradeAgain()
    {
        await PlayerCmd.LoseGold(DynamicVars.Gold.BaseValue, Owner!, GoldLossType.Stolen);

        await RollTrade(Owner.RunState.Rng.Niche.NextInt(100) < 40);
        SetEventState(L10NLookup($"{Id.Entry}.pages.CONTINUE.description"), ContinueOptions());
    }

    private async Task RollTrade(bool success)
    {
        if (success)
        {
            await PlayerCmd.GainGold(DynamicVars["WinGold"].BaseValue, Owner!);
        }
        else
        {
            await PlayerCmd.GainGold(DynamicVars["FailGold"].BaseValue, Owner!);
        }
    }

    // 继续交易场景的选项，钱不够时锁定继续交易
    private IReadOnlyList<EventOption> ContinueOptions()
    {
        var tradeAgain = Owner != null && Owner.Gold < DynamicVars.Gold.BaseValue
            ? new EventOption(this, null, ModOptionKey("CONTINUE", "TRADE_AGAIN"))
            : new EventOption(this, TradeAgain, ModOptionKey("CONTINUE", "TRADE_AGAIN"));
        return
        [
            tradeAgain,
            new EventOption(this, Go, ModOptionKey("CONTINUE", "GO"))
        ];
    }

    // 算了不打扰别人分分钟几百万上下了！获得眩晕，离开
    private async Task Leave()
    {
        await AddCardToDeck<Vertigo>();
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.LEAVE.description"));
    }

    // 走了！离开
    private Task Go()
    {
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.LEAVE.description"));
        return Task.CompletedTask;
    }

    private async Task AddCardToDeck<T>() where T : CardModel
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner!.RunState.CreateCard<T>(Owner), PileType.Deck), 2f);
    }
}