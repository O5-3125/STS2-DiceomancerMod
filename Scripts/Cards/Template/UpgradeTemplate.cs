using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interactions.RightClick;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Template;

public abstract class UpgradeTemplate<TTransform>(
    int energyCost,
    CardType type,
    CardRarity rarity,
    TargetType targetType,
    int upgradeCost)
    : ModCardTemplate(energyCost, type, rarity, targetType), IModRightClickableCard
    where TTransform : CardModel
{
    protected readonly int UpgradeCost = upgradeCost;

    protected override HashSet<CardTag> CanonicalTags => [MyTags.Upgrade.GetModCardTag()];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        new[]
        {
            HoverTipFactory.FromCard<TTransform>(),
            new HoverTip(new LocString("static_hover_tips", "upgrade.title"),
                new LocString("static_hover_tips", "upgrade.description"))
        }.Concat(OwnAdditionalHoverTips);


    protected virtual IEnumerable<IHoverTip> OwnAdditionalHoverTips => [];


    protected sealed override IEnumerable<DynamicVar> CanonicalVars =>
        OwnCanonicalVars.Append(new DynamicVar("Upgrade", UpgradeCost)
        );

    protected abstract IEnumerable<DynamicVar> OwnCanonicalVars { get; }

    public async Task OnRightClick(ModRightClickExecutionContext context)
    {
        var tech = Owner.Creature.GetPower<TechPower>();
        if (tech == null) return;
        var amount = tech.Amount;

        if (DynamicVars["Upgrade"].BaseValue <= amount)
        {
            await PowerCmd.ModifyAmount(context.PlayerChoiceContext, tech, -DynamicVars["Upgrade"].BaseValue, null,
                this);
            DynamicVars["Upgrade"].BaseValue = 0;
        }

        if (DynamicVars["Upgrade"].BaseValue <= 0)
        {
            CardModel cardModel = base.CombatState.CreateCard<TTransform>(base.Owner);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(cardModel);
            }

            await CardCmd.Transform(this, cardModel);
        }
    }
}