using Diceomancer.Scripts.Cards.Curse;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Events;

[RegisterActEvent(typeof(Glory))]
public sealed class Parade : ModEventTemplate
{
    // 背景图位置,目前没有背景图所以暂时用TestEvent.png代替
    public override EventAssetProfile AssetProfile => new(
        InitialPortraitPath: "res://Diceomancer/images/Event/TestEvent.png"
    );

    // 设置一些数值
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new GoldVar(120),
        new GoldVar("FailGold", 40),
        new StringVar("HesitationCard", ModelDb.Card<Hesitation>().Title),
        new StringVar("VertigoCard", ModelDb.Card<Vertigo>().Title)
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            new EventOption(this, KnifeShow, InitialOptionKey("KNIFE_SHOW"), HoverTipFactory.FromCard<Hesitation>()),
            new EventOption(this, HammerShow, InitialOptionKey("HAMMER_SHOW"), HoverTipFactory.FromCard<Vertigo>()),
            new EventOption(this, DiceShow, InitialOptionKey("DICE_SHOW"))
        ];
    }

    // 表演一个小刀拉屁股！获得犹豫和120金币
    private async Task KnifeShow()
    {
        await AddCardToDeck<Hesitation>();
        await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner!);
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.SUCCESS.description"));
    }

    // 表演一个大锤抡脑门！获得眩晕和120金币
    private async Task HammerShow()
    {
        await AddCardToDeck<Vertigo>();
        await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner!);
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.SUCCESS.description"));
    }

    // 表演一个神奇扔骰子！50%概率成功获得120金币，失败获得40金币
    private async Task DiceShow()
    {
        if (Owner.RunState.Rng.Niche.NextBool())
        {
            await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner!);
            SetEventFinished(L10NLookup($"{Id.Entry}.pages.SUCCESS.description"));
        }
        else
        {
            await PlayerCmd.GainGold(DynamicVars["FailGold"].BaseValue, Owner!);
            SetEventFinished(L10NLookup($"{Id.Entry}.pages.FAIL.description"));
        }
    }

    private async Task AddCardToDeck<T>() where T : CardModel
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner!.RunState.CreateCard<T>(Owner), PileType.Deck), 2f);
    }
}