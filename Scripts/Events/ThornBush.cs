using Diceomancer.Scripts.Cards.Event;
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
public sealed class ThornBush : ModEventTemplate
{
    // 背景图位置,目前没有背景图所以暂时用TestEvent.png代替
    public override EventAssetProfile AssetProfile => new(
        InitialPortraitPath: "res://Diceomancer/images/Event/TestEvent.png"
    );

    // 设置一些数值
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6m, ValueProp.Unblockable | ValueProp.Unpowered),
        new StringVar("ThornWallCard", ModelDb.Card<ThornWall>().Title),
        new StringVar("GlowBerriesCard", ModelDb.Card<GlowBerries>().Title)
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            new EventOption(this, TakeThorns, InitialOptionKey("THORNS"), HoverTipFactory.FromCard<ThornWall>()),
            new EventOption(this, Grab, InitialOptionKey("GRAB"), HoverTipFactory.FromCard<GlowBerries>()),
            new EventOption(this, Leave, InitialOptionKey("LEAVE"))
        ];
    }

    // 荆棘也是好东西，你收下了！获得荆棘墙
    private async Task TakeThorns()
    {
        await AddCardToDeck<ThornWall>();
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.THORNS_DONE.description"));
    }

    // 冲进去拿了！失去6生命，获得发光果
    private async Task Grab()
    {
        await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), Owner!.Creature, DynamicVars.Damage, null, null);
        await AddCardToDeck<GlowBerries>();
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.GRAB_DONE.description"));
    }

    // 走了！离开
    private Task Leave()
    {
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.LEAVE_DONE.description"));
        return Task.CompletedTask;
    }

    private async Task AddCardToDeck<T>() where T : CardModel
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner!.RunState.CreateCard<T>(Owner), PileType.Deck), 2f);
    }
}
