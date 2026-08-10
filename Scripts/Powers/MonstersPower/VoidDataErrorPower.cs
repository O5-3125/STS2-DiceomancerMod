using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Models.Capabilities;

namespace Diceomancer.Scripts.Powers.MonstersPower;

// 空图层被动：数据错误
// 失去生命和获得减益时改为获得等量最大生命
[RegisterPower]
public class VoidDataErrorPower : ModPowerTemplate
{
    private const int MaxHp = 1000;

    public override PowerType Type => PowerType.None;

    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/MonstersPower/{GetType().Name}.png",
        $"res://Diceomancer/images/Power/MonstersPower/{GetType().Name}.png"
    );

    public override decimal ModifyHpLostBeforeOsty(Creature target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource)
    {
        if (target != Owner)
        {
            return amount;
        }


        Flash();
        CreatureCmd.GainMaxHp(Owner, amount);

        return 0;
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power,
        decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power.GetTypeForAmount(amount) == PowerType.Debuff
            && power.Owner == Owner && power is not ITemporaryPower)
        {
            Flash();
            await CreatureCmd.GainMaxHp(Owner, amount);
        }
    }

    public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        if (Owner.CurrentHp > MaxHp)
        {
            await CreatureCmd.Kill(Owner);
        }
   
        
    }
}