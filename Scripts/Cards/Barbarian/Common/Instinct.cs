using Diceomancer.Scripts.Hero.Barbarian;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Barbarian.Common;

[RegisterCard(typeof(BarbarianCardPool))]
public class Instinct() : ModCardTemplate(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<Excitement>(3),
        new PowerVar<StrengthPower>(3)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        new HoverTip(new LocString("static_hover_tips", "vengeance.title"),
            new LocString("static_hover_tips", "vengeance.description")),
        HoverTipFactory.FromPower<Excitement>(),
        HoverTipFactory.FromPower<StrengthPower>()
    ];

    protected override bool ShouldGlowGoldInternal =>
        Owner.Creature.GetPowerAmount<Injury>() > 4;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var injury = Owner.Creature.GetPowerAmount<Injury>();

        if (injury > 4)
            await PowerCmd.Apply<Excitement>(choiceContext, Owner.Creature,
                DynamicVars["Excitement"].IntValue, Owner.Creature, this);

        if (injury > 8)
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature,
                DynamicVars["StrengthPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}