using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Builder.Rare;

[RegisterCard(typeof(BuilderCardPool))]
public class Mutation() : EvolutionTemplate(2, CardType.Skill, CardRarity.Rare, TargetType.Self, 2M)
{
    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
        new("Buff", 3),
        new RepeatVar(3)
    ];

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState, "base.CombatState");
        for (var i = 0; i < DynamicVars.Repeat.IntValue; i++)
            await DiceomancerCardCmd.ApplyRandomBuff(choiceContext, Owner, Owner.Creature,
                Owner.Creature, null, DynamicVars["Buff"].IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Evolution"].UpgradeValueBy(1);
    }
}