using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interactions.RightClick;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using STS2RitsuLib.CardTags;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class SteamEngine()
    : ModCardTemplate(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self), IModRightClickableCard
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override HashSet<CardTag> CanonicalTags => [MyTags.Upgrade.GetModCardTag()];

    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(2),
        new BlockVar(9, ValueProp.Move),
        new DynamicVar("Upgrade", 4)
            .WithSharedTooltip("upgrade")
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromCard<SummonTank>(),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);

        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
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
            CardModel cardModel = base.CombatState.CreateCard<SummonTank>(base.Owner);
            await CardCmd.Transform(this, cardModel);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Block.UpgradeValueBy(3m);
    }
}