using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers.Elements;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interactions.RightClick;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using STS2RitsuLib.CardTags;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class BatchSummon()
    : ModCardTemplate(2, CardType.Skill, CardRarity.Rare, TargetType.Self), IModRightClickableCard
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override HashSet<CardTag> CanonicalTags => [MyTags.Upgrade.GetModCardTag()];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromCard<StandardizedSummon>(),
        HoverTipFactory.FromPower<FireElement>(),
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<FireElement>(3),
        new DynamicVar("Upgrade", 3)
            .WithSharedTooltip("upgrade"),
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (int i = 0; i < DynamicVars["FireElement"].IntValue; i++)
        {
            await PowerCmd.Apply<FireElement>(choiceContext, base.Owner.Creature, DynamicVars.Summon.IntValue,
                base.Owner.Creature, this);
        }
    }

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
            CardModel cardModel = base.CombatState.CreateCard<StandardizedSummon>(base.Owner);
            await CardCmd.Transform(this, cardModel);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars["FireElement"].UpgradeValueBy(1m);
    }
}