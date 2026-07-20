using Diceomancer.Scripts.Capabilitys;
using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interactions.RightClick;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;
using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using STS2RitsuLib.CardTags;

namespace Diceomancer.Scripts.Cards.Common;

[RegisterCard(typeof(DiceomancerCardPool))]
public class GunBarrel() :
    ModCardTemplate(2, CardType.Attack, CardRarity.Common, TargetType.AllEnemies), IModRightClickableCard
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override HashSet<CardTag> CanonicalTags => [MyTags.Upgrade.GetModCardTag()];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(12, ValueProp.Move),
        new PowerVar<PlatingPower>(4),
        new DynamicVar("Upgrade", 3)
            .WithSharedTooltip("upgrade")
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromCard<Cannon>(),
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(Owner.Creature.CombatState);

        await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(Owner.Creature.CombatState)
            .Execute(choiceContext);

        await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature,
            base.DynamicVars["PlatingPower"].BaseValue, base.Owner.Creature, this);
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
            CardModel cardModel = base.CombatState.CreateCard<Cannon>(base.Owner);
            await CardCmd.Transform(this, cardModel);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}