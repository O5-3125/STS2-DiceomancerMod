using Diceomancer.Scripts.Cards.Event;
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
public sealed class HammerWall : ModEventTemplate
{
    // 背景图位置,目前没有背景图所以暂时用TestEvent.png代替
    public override EventAssetProfile AssetProfile => new(
        InitialPortraitPath: "res://Diceomancer/images/Event/TestEvent.png"
    );

    // 设置一些数值
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new MaxHpVar(12),
        new MaxHpVar("FixMaxHp", 20),
        new StringVar("HammerCard", ModelDb.Card<BreachingHammer>().Title),
        new StringVar("PlateCard", ModelDb.Card<Reinforcement>().Title)
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            new EventOption(this, Hammer, InitialOptionKey("HAMMER"), HoverTipFactory.FromCard<BreachingHammer>()),
            new EventOption(this, Fix, InitialOptionKey("FIX"), HoverTipFactory.FromCard<Reinforcement>())
        ];
    }

    // 我要去锤墙的一方！获得12最大生命值和破墙大锤
    private async Task Hammer()
    {
        await CreatureCmd.GainMaxHp(Owner!.Creature, DynamicVars.MaxHp.BaseValue);
        await AddCardToDeck<BreachingHammer>();
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.END.description"));
    }

    // 我要去修墙的一方！获得20最大生命值和加固板
    private async Task Fix()
    {
        await CreatureCmd.GainMaxHp(Owner!.Creature, DynamicVars["FixMaxHp"].BaseValue);
        await AddCardToDeck<Reinforcement>();
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.END.description"));
    }

    private async Task AddCardToDeck<T>() where T : CardModel
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner!.RunState.CreateCard<T>(Owner), PileType.Deck), 2f);
    }
}
