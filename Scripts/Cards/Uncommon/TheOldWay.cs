using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class TheOldWay() : ModCardTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(3),

        new("maxEnergy", 3),

        new PowerVar<StrengthPower>(3)
    ];

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        return 3;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ModifyMaxEnergy(this.Owner, DynamicVars["maxEnergy"].IntValue);

        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);

        await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner.Creature,
            DynamicVars["StrengthPower"].IntValue, this.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.RemoveKeyword(CardKeyword.Exhaust);
    }
}