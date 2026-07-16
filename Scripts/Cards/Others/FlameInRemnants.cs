using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Rare;

// [RegisterCard(typeof(DiceomancerCardPool))]
public class FlameInRemnants() : ModCardTemplate(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy, true)
{
    // protected override bool HasEnergyCostX => true;


    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    // protected override IEnumerable<DynamicVar> CanonicalVars => [];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        int amount = this.Owner.Creature.MaxHp - this.Owner.Creature.CurrentHp;

        await PowerCmd.Apply<BurnPower>(choiceContext, cardPlay.Target, amount, this.Owner.Creature, this);
    }


    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}