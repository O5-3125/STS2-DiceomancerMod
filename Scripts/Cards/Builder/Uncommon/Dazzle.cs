using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Builder.Uncommon;

[RegisterCard(typeof(BuilderCardPool))]
public class Dazzle() :
    KickTemplate(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, 4)
{
    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
        new("Debuff", 4)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DiceomancerCardCmd.ApplyRandomDebuff(choiceContext, Owner, cardPlay.Target,
            Owner.Creature, this, DynamicVars["Debuff"].IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Kick"].BaseValue += 6;
    }
}
