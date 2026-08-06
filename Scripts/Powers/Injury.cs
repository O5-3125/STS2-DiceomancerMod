using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Combat.HealthBars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers;

[RegisterPower]
public class Injury : ModPowerTemplate, IHealthBarForecastSource
{
    // 类型，Buff或Debuff
    public override PowerType Type => PowerType.Debuff;

    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType => PowerStackType.Counter;

  public override PowerAssetProfile AssetProfile => new(
    $"res://Diceomancer/images/Power/{GetType().Name}.png",
    $"res://Diceomancer/images/Power/{GetType().Name}.png"
);

    // 覆盖生命血条
    public IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return HealthBarForecasts.Single(
            context.Creature.GetPowerAmount<Injury>(), // 展示的数量（例如如果你的能力有2倍效果可以乘2）
            new Color(1f, 0.647f, 0f), // 颜色
            HealthBarForecastGrowthDirection.FromRight // 从左边开始延伸还是右边开始
            // 0, // 顺序，越大越远离血条边缘，默认0
            // PreloadManager.Cache.GetMaterial("res://xxx.tres") // 如果需要自定义材质
        );
    }

    // 回合结束后
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner)) return;

        Flash();
        await CreatureCmd.Damage(choiceContext, Owner, Amount,
            ValueProp.Unblockable | ValueProp.Unpowered, null, null);

        if (side == Owner.Side)
            // await PowerCmd.Remove(this);
            await PowerCmd.ModifyAmount(choiceContext, this, -(Amount / 2+ 1), Owner, null);
    }
}