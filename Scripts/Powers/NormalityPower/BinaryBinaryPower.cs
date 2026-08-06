using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.NormalityPower;

[RegisterPower]
public class BinaryBinaryPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.None;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/{GetType().Name}.png",
        $"res://Diceomancer/images/Power/{GetType().Name}.png"
    );

    protected override bool IsVisibleInternal => false;

    public override async Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
    {
        await PowerCmd.Remove<BinaryBinaryPower>(target);
    }

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner)) return;
        await PowerCmd.Remove(this);
    }

    public override decimal ModifyDamageAdditive(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource, CardPlay? cardPlay)
    {
        if (base.Owner != dealer) return 0m;
        return ReplaceDigits((int)amount, (int)base.Amount) - amount;
    }

    public static int ReplaceDigits(int num, int digit)
    {
        if (digit <= 0 || num == 0) return 0;
        if (digit > 9) digit = 9;

        var result = 0;
        var multiplier = 1;
        while (num > 0)
        {
            result += digit * multiplier;
            multiplier *= 10;
            num /= 10;
        }

        return result;
    }
}