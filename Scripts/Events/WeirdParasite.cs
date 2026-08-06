using Diceomancer.Scripts.Cards.Curse;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Events;

[RegisterActEvent(typeof(Hive))]
public sealed class WeirdParasite : ModEventTemplate
{
    // 背景图位置,目前没有背景图所以暂时用TestEvent.png代替
    public override EventAssetProfile AssetProfile => new(
        InitialPortraitPath: "res://Diceomancer/images/Event/TestEvent.png"
    );

    // 设置一些数值
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HealVar(24),
        new MaxHpVar(10),
        new DamageVar(3m, ValueProp.Unblockable | ValueProp.Unpowered),
        new StringVar("VertigoCard", ModelDb.Card<Vertigo>().Title)
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            new EventOption(this, Eat, InitialOptionKey("EAT"), HoverTipFactory.FromCard<Vertigo>()),
            new EventOption(this, LetIn, InitialOptionKey("LET_IN"), HoverTipFactory.FromCard<Vertigo>()),
            new EventOption(this, Run, InitialOptionKey("RUN"))
        ];
    }

    // 鸡肉味嘎嘣脆！获得24生命和眩晕
    private async Task Eat()
    {
        await CreatureCmd.Heal(Owner!.Creature, DynamicVars.Heal.BaseValue);
        await AddCardToDeck<Vertigo>();
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.EAT_DONE.description"));
    }

    // 任由寄生虫钻进来！获得10最大生命值和眩晕
    private async Task LetIn()
    {
        await CreatureCmd.GainMaxHp(Owner!.Creature, DynamicVars.MaxHp.BaseValue);
        await AddCardToDeck<Vertigo>();
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.LET_IN_DONE.description"));
    }

    // 好恶心快跑啊！失去3生命
    private async Task Run()
    {
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner!.Creature, DynamicVars.Damage, null, null);
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.RUN_DONE.description"));
    }

    private async Task AddCardToDeck<T>() where T : CardModel
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner!.RunState.CreateCard<T>(Owner), PileType.Deck), 2f);
    }
}
