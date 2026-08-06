using Diceomancer.Scripts.Cards.Curse;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Events;

[RegisterActEvent(typeof(Underdocks))]
public sealed class ConstructionSite : ModEventTemplate
{
    // 背景图位置,目前没有背景图所以暂时用TestEvent.png代替
    public override EventAssetProfile AssetProfile => new(
        InitialPortraitPath: "res://Diceomancer/images/Event/TestEvent.png"
    );

    // 设置一些数值
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new GoldVar(40),
        new GoldVar("HardGold", 90),
        new StringVar("ExhaustedCard", ModelDb.Card<Exhausted>().Title)
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            new EventOption(this, Help, InitialOptionKey("HELP")),
            new EventOption(this, HelpHard, InitialOptionKey("HELP_HARD"), HoverTipFactory.FromCard<Exhausted>())
        ];
    }

    // 那就帮帮你吧！获得40金币
    private async Task Help()
    {
        await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner!);
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.LEAVE_1.description"));
    }

    // 我力气可大了，给你干得明明白白的！获得疲惫和90金币
    private async Task HelpHard()
    {
        await AddCardToDeck<Exhausted>();
        await PlayerCmd.GainGold(DynamicVars["HardGold"].BaseValue, Owner!);
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.LEAVE_2.description"));
    }

    private async Task AddCardToDeck<T>() where T : CardModel
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner!.RunState.CreateCard<T>(Owner), PileType.Deck), 2f);
    }
}
