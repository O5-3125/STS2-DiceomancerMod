using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Template;

// 流转
public abstract class FlowTemplate(int energyCost, CardType type, CardRarity rarity, TargetType targetType)
    : ModCardTemplate(energyCost, type, rarity, targetType)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        new HoverTip(new LocString("static_hover_tips", "flow.title"),
            new LocString("static_hover_tips", "flow.description"))
    ];

    protected bool Flow { get; set; } = true;

    protected override bool ShouldGlowGoldInternal => Flow;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay.Card;
        if (card.Owner != Owner) return;

        Flow = card.Type != Type;
    }
}