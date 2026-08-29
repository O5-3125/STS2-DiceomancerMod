using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Common.Patches;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Template;

public abstract class UpgradeTemplate<TTransform>(
    int energyCost,
    CardType type,
    CardRarity rarity,
    TargetType targetType,
    int upgradeCost)
    : ModCardTemplate(energyCost, type, rarity, targetType)
    where TTransform : CardModel
{
    protected readonly int UpgradeCost = upgradeCost;

    protected override HashSet<CardTag> CanonicalTags => [MyTags.Upgrade.GetModCardTag()];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        new[]
        {
            HoverTipFactory.FromCard<TTransform>(IsUpgraded),
            new HoverTip(new LocString("static_hover_tips", "upgrade.title"),
                new LocString("static_hover_tips", "upgrade.description"))
        }.Concat(OwnAdditionalHoverTips);


    protected virtual IEnumerable<IHoverTip> OwnAdditionalHoverTips => [];


    protected sealed override IEnumerable<DynamicVar> CanonicalVars =>
        OwnCanonicalVars.Append(new DynamicVar("Upgrade", UpgradeCost)
        );

    protected abstract IEnumerable<DynamicVar> OwnCanonicalVars { get; }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var played = cardPlay.Card;
        if (played == this || played.Owner != Owner) return;
        if (!ChargedStrikeNeighborTracker.GetNeighbors(played).Contains(this)) return;
        if (!cardPlay.IsLastInSeries) return;

        DynamicVars["Upgrade"].BaseValue -= 1;

        if (DynamicVars["Upgrade"].BaseValue <= 0)
        {
            CardModel cardModel = CombatState.CreateCard<TTransform>(Owner);
            if (IsUpgraded) CardCmd.Upgrade(cardModel);

            await CardCmd.Transform(this, cardModel);
        }
    }
}