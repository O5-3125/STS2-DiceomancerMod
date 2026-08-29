using Diceomancer.Scripts.Cards.Event;
using Diceomancer.Scripts.Potions;
using Diceomancer.Scripts.Relics.Event;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Runs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Events;

[RegisterSharedEvent]
public sealed class MagicGenius : ModEventTemplate
{
    // 背景图位置,目前没有背景图所以暂时用TestEvent.png代替
    public override EventAssetProfile AssetProfile => new(
        InitialPortraitPath: "res://Diceomancer/images/Event/TestEvent.png"
    );

    public override bool IsAllowed(IRunState runState) => runState.CurrentActIndex == 0;
    // 设置一些数值
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new StringVar("PunchingTheAirCard", ModelDb.Card<PunchingTheAir>().Title),
    ];

    private static readonly IReadOnlyList<Func<MagicGenius, EventOption>> AllOptionFactories =
    [
        self => new EventOption(self, self.LearnAir, self.InitialOptionKey("LEARN_AIR"),
            HoverTipFactory.FromCard<PunchingTheAir>()),
        self => new EventOption(self, self.LearnCharge, self.InitialOptionKey("LEARN_CHARGE"),
            HoverTipFactory.FromRelic<PowerBank>()),
        self => new EventOption(self, self.LearnIdea, self.InitialOptionKey("LEARN_IDEA"),
            HoverTipFactory.FromRelic<ChaosPendant>()),
        // self => new EventOption(self, self.Bargain, self.InitialOptionKey("BARGAIN"),
        //     HoverTipFactory.FromRelic<BlackCard>()),
        self => new EventOption(self, self.GetBag, self.InitialOptionKey("GET_BAG"),
            HoverTipFactory.FromRelic<DimensionalBag>()),

        self => new EventOption(self, self.MundanePotion, self.InitialOptionKey("MUNDANE_POTION"),
            HoverTipFactory.FromPotion<MundanePotion>()),
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        // 从所有选项池中随机列出三个
        var pool = AllOptionFactories.ToList();
        var rng = Owner!.RunState.Rng.Niche;
        for (var i = pool.Count - 1; i > 0; i--)
        {
            var j = rng.NextInt(i + 1);
            (pool[i], pool[j]) = (pool[j], pool[i]);
        }

        return pool.Take(3).Select(factory => factory(this)).ToList();
    }

    // 我学学这个刮痧术：获得刮痧术
    private async Task LearnAir()
    {
        await AddCardToDeck<PunchingTheAir>();
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.DONE.description"));
    }

    // 我学学这个充电：获得聚能手环
    private async Task LearnCharge()
    {
        await RelicCmd.Obtain(ModelDb.Relic<PowerBank>().ToMutable(), Owner!);
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.DONE.description"));
    }

    // 我学学您的魔法思路：获得混沌吊坠
    private async Task LearnIdea()
    {
        await RelicCmd.Obtain(ModelDb.Relic<ChaosPendant>().ToMutable(), Owner!);
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.DONE.description"));
    }

    // 相见即是缘分，你给我打个折吧：获得很黑的卡
    private async Task Bargain()
    {
        await RelicCmd.Obtain(ModelDb.Relic<BlackCard>().ToMutable(), Owner!);
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.DONE.description"));
    }

    // 相见即是缘分，你给我打个折吧：获得不平凡的药水
    private async Task MundanePotion()
    {
        await PotionCmd.TryToProcure(ModelDb.Potion<MundanePotion>().ToMutable(), Owner!);
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.DONE.description"));
    }

    // 这个次元袋看着很实用：获得次元袋
    private async Task GetBag()
    {
        await RelicCmd.Obtain(ModelDb.Relic<DimensionalBag>().ToMutable(), Owner!);
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.DONE.description"));
    }

    private async Task AddCardToDeck<T>() where T : CardModel
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner!.RunState.CreateCard<T>(Owner), PileType.Deck), 2f);
    }
}