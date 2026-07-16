using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Event;

[RegisterCard(typeof(EventCardPool))]
public class ChaosSpell() : ModCardTemplate(0, CardType.Skill, CardRarity.Event, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        IEnumerable<PowerModel> powerList =
            base.Owner.Creature.Powers
                .Where(p => p is { StackType: PowerStackType.Counter, Type: PowerType.Buff })
                .ToList();

        IEnumerable<int> powerAmountList =
            powerList.Select(p => p.Amount).ToList().StableShuffle(base.Owner.RunState.Rng.Shuffle);

        foreach (var power in powerList)
        {
            await PowerCmd.Remove(power);
        }

        for (var i = 0; i < powerList.Count(); i++)
        {
            await PowerCmd.Apply(choiceContext, powerList.ElementAt(i), Owner.Creature,
                powerAmountList.ElementAt(i), Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        this.RemoveKeyword(CardKeyword.Exhaust);
    }
}