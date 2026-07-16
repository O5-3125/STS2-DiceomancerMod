using Diceomancer.Scripts.Cards.Basic;
using Diceomancer.Scripts.Cards.Rare;
using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.CardPool;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interactions.RightClick;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Common;

[RegisterCard(typeof(DiceomancerCardPool))]
public sealed class Spike()
    : ModCardTemplate(2, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy), IModRightClickableCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<WeakPower>(2),
        new PowerVar<BleedPower>(8),
        new DynamicVar("Upgrade", 2)
            .WithSharedTooltip("upgrade")
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<BleedPower>(),

        HoverTipFactory.FromCard<SpikeTrap>(),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target,
            DynamicVars.Weak.IntValue, Owner.Creature, this);

        await PowerCmd.Apply<BleedPower>(choiceContext, cardPlay.Target,
            DynamicVars["BleedPower"].IntValue, Owner.Creature, this);
    }
    public async Task OnRightClick(ModRightClickExecutionContext context)
    {
        var tech = Owner.Creature.GetPower<TechPower>();
        if (tech == null) return;
        var amount = tech.Amount;

        if (DynamicVars["Upgrade"].BaseValue > amount)
        {
            await PowerCmd.Remove(tech);
            DynamicVars["Upgrade"].BaseValue -= amount;
        }
        else
        {
            await PowerCmd.ModifyAmount(context.PlayerChoiceContext, tech, -DynamicVars["Upgrade"].BaseValue, null,
                this);
            DynamicVars["Upgrade"].BaseValue = 0;
        }

        if (DynamicVars["Upgrade"].BaseValue <= 0)
        {
            CardModel cardModel = base.CombatState.CreateCard<SpikeTrap>(base.Owner);
            await CardCmd.Transform(this, cardModel);
        }
    }
    // public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    // {
    //     if (cardPlay.Card == this) return;
    //     if (Pile?.Type != PileType.Hand) return;
    //
    //     DynamicVars["Upgrade"].BaseValue--;
    //
    //     ArgumentNullException.ThrowIfNull(base.CombatState);
    //     if (DynamicVars["Upgrade"].BaseValue <= 0)
    //     {
    //         CardModel cardModel = base.CombatState.CreateCard<SpikeTrap>(base.Owner);
    //         await CardCmd.Transform(this, cardModel);
    //     }
    // }


    protected override void OnUpgrade()
    {
        base.DynamicVars.Weak.UpgradeValueBy(1);
        base.DynamicVars["BleedPower"].UpgradeValueBy(2);
    }
}