using Diceomancer.Scripts.Cards.Basic;
using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers.NormalityPower;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interactions.RightClick;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Uncommon;
[RegisterCard(typeof(DiceomancerCardPool))]

public class FallIn()
    : ModCardTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self), IModRightClickableCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new SummonVar(8),
        new DynamicVar("Upgrade", 4)
            .WithSharedTooltip("upgrade")
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromCard<TheTimeIsCalling>(),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await OstyCmd.Summon(choiceContext, Owner, DynamicVars.Summon.IntValue, this);
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
            CardModel cardModel = base.CombatState.CreateCard<TheTimeIsCalling>(base.Owner);
            await CardCmd.Transform(this, cardModel);
        }
    }
    //
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
    //         CardModel cardModel = base.CombatState.CreateCard<TheTimeIsCalling>(base.Owner);
    //         await CardCmd.Transform(this, cardModel);
    //     }
    // }


    protected override void OnUpgrade()
    {
        base.DynamicVars["Upgrade"].UpgradeValueBy(1m);
    }
}