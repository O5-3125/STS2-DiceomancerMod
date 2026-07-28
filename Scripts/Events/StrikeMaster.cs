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
public sealed class StrikeMaster : ModEventTemplate
{
    // 背景图位置
    public override EventAssetProfile AssetProfile => new(
        // InitialPortraitPath: $"res://Diceomancer/images/Event/{GetType().Name}.png",
        InitialPortraitPath: "res://Diceomancer/images/Event/TestEvent.png"
    );

    // 设置一些数值
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar("SmallStrike", 1),
        new CardsVar("LargeStrike", 4),
        new StringVar("ExcellentStrike", ModelDb.Card<ExcellentStrike>().Title),
        new StringVar("InvincibleStrike", ModelDb.Card<InvincibleStrike>().Title)
    ];

    protected override IReadOnlyList<EventOption> GenerateInitialOptions()
    {
        return
        [
            new EventOption(this, ExcellentStrike, InitialOptionKey("EXCELLENT_STRIKE"),
                HoverTipFactory.FromCard<ExcellentStrike>()),
            new EventOption(this, InvincibleStrike, InitialOptionKey("INVINCIBLE_STRIKE"),
                HoverTipFactory.FromCard<InvincibleStrike>()),

            new EventOption(this, NoStrike, InitialOptionKey("NO_STRIKE"))
        ];
    }

    // 优秀打击
    private async Task ExcellentStrike()
    {
        for (int i = 0; i < DynamicVars["SmallStrike"].IntValue; i++)
        {
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(
                    base.Owner.RunState.CreateCard(GetStrikeForCharacter(base.Owner.Character), base.Owner),
                    PileType.Deck),
                2f);
        }

        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(base.Owner.RunState.CreateCard<ExcellentStrike>(base.Owner),
            PileType.Deck));
        StrikeChosen();
    }

    // 无敌打击
    private async Task InvincibleStrike()
    {
        for (int i = 0; i < DynamicVars["LargeStrike"].IntValue; i++)
        {
            CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(
                    base.Owner.RunState.CreateCard(GetStrikeForCharacter(base.Owner.Character), base.Owner),
                    PileType.Deck),
                2f);
        }

        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(base.Owner.RunState.CreateCard<InvincibleStrike>(base.Owner),
            PileType.Deck));
        StrikeChosen();
    }

    private async Task NoStrike()
    {
    }

    private static CardModel GetStrikeForCharacter(CharacterModel character)
    {
        return character.CardPool.AllCards.First(c =>
            c.Rarity == CardRarity.Basic && c.Tags.Contains(CardTag.Strike));
    }

    private void StrikeChosen()
    {
        SetEventFinished(L10NLookup($"{Id.Entry}.pages.STRIKE_CHOSEN.description"));
    }
}