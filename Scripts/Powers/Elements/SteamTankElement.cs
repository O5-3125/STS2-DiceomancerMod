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
public class SteamTankElement : ElementTemplate<EssenceOfWater>
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(20, ValueProp.Unpowered),
        new BlockVar(10, ValueProp.Unpowered)
    ];

    public override async Task Evoke(PlayerChoiceContext choiceContext)
    {
        ArgumentNullException.ThrowIfNull(Owner.CombatState);
        await CreatureCmd.Damage(choiceContext, base.CombatState.HittableEnemies,
            DynamicVars.Damage.IntValue, ValueProp.Unpowered, base.Owner);
        await CreatureCmd.GainBlock(Owner, DynamicVars.Block, null);
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