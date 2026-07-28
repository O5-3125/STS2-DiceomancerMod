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
    // protected override HashSet<CardTag> CanonicalTags => [MyTags.Modify.GetModCardTag()];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        new HoverTip(new LocString("static_hover_tips", "miracle.title"),
            new LocString("static_hover_tips", "miracle.description"))
    ];

    protected bool Miracle { get; set; } = true;

    protected override bool ShouldGlowGoldInternal => Miracle;

    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this) return Task.CompletedTask;

        Miracle = !fromHandDraw;
        return Task.CompletedTask;
    }

    public override Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card != this) return Task.CompletedTask;
        Miracle = true;

        return Task.CompletedTask;
    }

    public override Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        Miracle = true;
        return Task.CompletedTask;
    }

    public override Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
    {
        if (card == this)
            Miracle = true;

        return Task.CompletedTask;
    }
}