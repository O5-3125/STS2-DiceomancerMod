using Diceomancer.Scripts.Cards.Token.Essence;
using Diceomancer.Scripts.Common.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Powers.Elements;

[RegisterPower]
public class BlackElement :ElementTemplate<EssenceOfBlack>
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new("Buff", 3)
    ];

    // 回合结束
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(base.Owner)) return;
        Flash();


        ArgumentNullException.ThrowIfNull(Owner.CombatState);
        var enemy = base.Owner.CombatState.RunState.Rng.CombatTargets.NextItem(base.CombatState.HittableEnemies);
        if (enemy == null) return;

        await DiceomancerCardCmd.ApplyRandomDebuff(choiceContext, Owner.Player, enemy, null,
            null, DynamicVars["Buff"].IntValue);
        await PowerCmd.Decrement(this);
    }
}