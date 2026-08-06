using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Event;

[RegisterCard(typeof(EventCardPool))]
public class Blur() : ModCardTemplate(2, CardType.Skill, CardRarity.Event, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<EvadePower>(4),
        new PowerVar<PlatingPower>(4)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<EvadePower>(choiceContext, Owner.Creature,
            DynamicVars["EvadePower"].IntValue, Owner.Creature, this);

        await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature,
            DynamicVars["PlatingPower"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}