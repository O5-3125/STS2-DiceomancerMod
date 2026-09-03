using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Template;

public abstract class MiracleTemplate(int energyCost, CardType type, CardRarity rarity, TargetType targetType)
    : ModCardTemplate(energyCost, type, rarity, targetType)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        new HoverTip(new LocString("static_hover_tips", "miracle.title"),
            new LocString("static_hover_tips", "miracle.description"))
    ];

    protected bool Miracle { get; set; } = true;

    protected override bool ShouldGlowGoldInternal => Miracle;

    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this) return;

        Miracle = !fromHandDraw;
    }
 
    public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel? clonedBy)
    {
        if (card != this) return;
        if (card.Pile == null || card.Pile.Type == PileType.Deck) return;
        if (card.Pile.Type == PileType.Play) return;


        if (card.Pile != PileType.Hand.GetPile(Owner))
            Miracle = true;
    }
}