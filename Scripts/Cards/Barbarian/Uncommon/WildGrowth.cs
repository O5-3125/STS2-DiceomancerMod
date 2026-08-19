using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero.Barbarian;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Barbarian.Uncommon;

[RegisterCard(typeof(BarbarianCardPool))]
public class WildGrowth() : ModCardTemplate(3, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, MyKeywords.Limited];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new MaxHpVar(4),
        new HealVar(4)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(6);
    }
}