using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interactions.RightClick;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Night()
    : ModCardTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy), IModRightClickableCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<BlindPower>(1),
        new DynamicVar("Upgrade", 3)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromCard<Dawn>(),
        HoverTipFactory.FromPower<BlindPower>(),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<BlindPower>(choiceContext, cardPlay.Target,
            DynamicVars["BlindPower"].IntValue, Owner.Creature, this);
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
            CardModel cardModel = base.CombatState.CreateCard<Dawn>(base.Owner);
            await CardCmd.Transform(this, cardModel);
        }
    }

    public override TargetType TargetType => Target;
    private TargetType Target { get; set; } = TargetType.AnyEnemy;

    protected override void OnUpgrade()
    {
        Target = TargetType.AllEnemies;
    }
}