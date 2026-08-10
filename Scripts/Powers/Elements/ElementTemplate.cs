using Diceomancer.Scripts.Common.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.Elements;


public abstract class ElementTemplate<TTransform>: ModPowerTemplate where TTransform : CardModel
{
    // 类型，Buff或Debuff
    public override PowerType Type => PowerType.Buff;

    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;


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

    public override async Task AfterRemoved(Creature oldOwner)
    {

        await CardPileCmd.AddToCombatAndPreview<TTransform>(Owner, PileType.Hand, 1, null);
    }

    
}