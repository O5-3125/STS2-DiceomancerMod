using Diceomancer.Scripts.Cards.Curse;
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

[RegisterActEvent(typeof(Overgrowth))]
public sealed class GymCabin : ModEventTemplate
{
    // 背景图位置,目前没有背景图所以暂时用TestEvent.png代替
    public override EventAssetProfile AssetProfile => new(
        InitialPortraitPath: "res://Diceomancer/images/Event/TestEvent.png"
    );

    // 设置一些数值
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new MaxHpVar(8),
        new MaxHpVar("HardMaxHp", 12),
        new MaxHpVar("MuscleMaxHp", 20),
        new CardsVar("MuscleSmash", 3),
        new StringVar("ExhaustedCard", ModelDb.Card<Exhausted>().Title),
        new StringVar("MuscleSmashCard", ModelDb.Card<MuscleSmash>().Title)
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            new EventOption(this, Train, InitialOptionKey("TRAIN")),
            new EventOption(this, TrainHard, InitialOptionKey("TRAIN_HARD"), HoverTipFactory.FromCard<Exhausted>()),
            new EventOption(this, Muscle, InitialOptionKey("MUSCLE"), HoverTipFactory.FromCard<MuscleSmash>())
        ];
    }

    // 那就去练练！获得8最大生命值
    private async Task Train()
    {
        await CreatureCmd.GainMaxHp(Owner!.Creature, DynamicVars.MaxHp.BaseValue);
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.LEAVE.description"));
    }

    // 免费不得练到昏厥！获得12最大生命值和疲惫
    private async Task TrainHard()
    {
        await CreatureCmd.GainMaxHp(Owner!.Creature, DynamicVars["HardMaxHp"].BaseValue);
        await AddCardToDeck<Exhausted>();
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.LEAVE.description"));
    }

    // 狠狠地进行肌肉训练！获得20最大生命值和肌肉猛击×3
    private async Task Muscle()
    {
        await CreatureCmd.GainMaxHp(Owner!.Creature, DynamicVars["MuscleMaxHp"].BaseValue);
        for (int i = 0; i < DynamicVars["MuscleSmash"].IntValue; i++)
        {
            await AddCardToDeck<MuscleSmash>();
        }
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.LEAVE.description"));
    }

    private async Task AddCardToDeck<T>() where T : CardModel
    {
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(Owner!.RunState.CreateCard<T>(Owner), PileType.Deck), 2f);
    }
}
