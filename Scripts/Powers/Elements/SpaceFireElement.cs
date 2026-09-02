using Diceomancer.Scripts.Cards.Token;
using Diceomancer.Scripts.Cards.Token.Essence;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.Elements;

[RegisterPower]
public class SpaceFireElement : ElementTemplate<EssenceOfFire>
{
    public override PowerAssetProfile AssetProfile => new(
        "res://Diceomancer/images/Power/Element/FireElement.png",
        "res://Diceomancer/images/Power/Element/FireElement.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6, ValueProp.Unpowered),
        new RepeatVar(6)
    ];

    public override async Task Evoke(PlayerChoiceContext choiceContext)
    {
        ArgumentNullException.ThrowIfNull(Owner.CombatState);
        var enemy = base.Owner.CombatState.RunState.Rng.CombatTargets.NextItem(base.CombatState.HittableEnemies);
        if (enemy == null) return;

        for (int i = 0; i < DynamicVars.Repeat.IntValue; i++)
        {
            await CreatureCmd.Damage(choiceContext, enemy,
                DynamicVars.Damage.IntValue, ValueProp.Unpowered, base.Owner);
        }

        await PowerCmd.Decrement(this);
    }

    // 回合结束
    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(base.Owner)) return;

        await Evoke(choiceContext);
    }
}