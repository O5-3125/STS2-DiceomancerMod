using Diceomancer.Scripts.Hero.Barbarian;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.Berserker;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interactions.RightClick;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Starter;

[RegisterRelic(typeof(BarbarianRelicPool))]
public class RedPurpleMana : ModRelicTemplate, IModRightClickableCard
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    // 小图标（原版85x85）
    public override string PackedIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";

    // 轮廓图标（原版85x85）
    protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";

    // 大图标（原版256x256）
    protected override string BigIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<Injury>()
    ];

    public override async Task AfterModifyingHpLostAfterOsty()
    {
        Flash();
        await PowerCmd.Apply<Injury>(new ThrowingPlayerChoiceContext(), Owner.Creature, injuryAmount, null, null);
    }

    private decimal injuryAmount;

    public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner.Creature || props.HasFlag(ValueProp.Unblockable) || amount == 0)
        {
            return amount;
        }

        injuryAmount = amount;

        return 0m;
    }

    public Task OnRightClick(ModRightClickExecutionContext context)
    {
        IEnumerable<PowerModel> powerList =
            Owner.Creature.Powers
                .Where(p => p is { StackType: PowerStackType.Counter })
                .ToList();

        IEnumerable<int> powerAmountList =
            powerList.Select(p => p.Amount).ToList().StableShuffle(Owner.RunState.Rng.Shuffle);


        for (var i = 0; i < powerList.Count(); i++)
            powerList.ElementAt(i).SetAmount(powerAmountList.ElementAt(i));

        return Task.CompletedTask;
    }
}