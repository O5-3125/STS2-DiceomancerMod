using Diceomancer.Scripts.Hero;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards;
using STS2RitsuLib.Combat.HealthBars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace Diceomancer.Scripts.Relics.Basic;
//
// // 加入角色遗物池
// [RegisterRelic(typeof(DiceomancerRelicPool))]
// // 加入初始遗物池
// [RegisterCharacterStarterRelic(typeof(DiceomancerCharacter))]
public class RedBall : ModRelicTemplate, ICardOnPlayHookListener
{
    private static readonly SavedAttachedState<RedBall, int> Injury = new("Injury", _ => 0);
    public override RelicRarity Rarity => RelicRarity.Common;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1),
        new("Injury", Injury[this])
    ];

    public override bool ShowCounter => true;
    public override int DisplayAmount => DynamicVars["Injury"].IntValue;

    // 小图标（原版85x85）
    public override string PackedIconPath => "res://Diceomancer/images/Relics/RedBall.png";

    // 轮廓图标（原版85x85）
    protected override string PackedIconOutlinePath => "res://Diceomancer/images/Relics/RedBall.png";

    // 大图标（原版256x256）
    protected override string BigIconPath => "res://Diceomancer/images/Relics/RedBall.png";

    // public Task<bool> BeforeCardOnPlay(BeforeCardOnPlayContext context)
    // {
    //     return Task.FromResult(context.CardPlay.Card.TargetType == TargetType.AnyEnemy);
    // }

    
    // 覆盖生命血条
    public IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return HealthBarForecasts.Single(
            Injury[this], // 展示的数量（例如如果你的能力有2倍效果可以乘2）
            new Color(1f, 0.647f, 0f), // 颜色
            HealthBarForecastGrowthDirection.FromRight // 从左边开始延伸还是右边开始
            // 0, // 顺序，越大越远离血条边缘，默认0
            // PreloadManager.Cache.GetMaterial("res://xxx.tres") // 如果需要自定义材质
        );
    }


    public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner.Creature || props.HasFlag(ValueProp.Unblockable) || amount == 0) return amount;

        Injury[this] += (int)amount;
        UpdateDisplay();
        return 0m;
    }

    // 回合结束后
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Creature.Side) return;

        if (Injury[this] == 0) return;

        // await CreatureCmd.Damage(choiceContext, base.Owner.Creature, DynamicVars["Injury"].IntValue,
        // ValueProp.Unblockable | ValueProp.Unpowered, null, null);

        Injury[this] /= 2;
        UpdateDisplay();
    }

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        Injury[this] /= 2;
        UpdateDisplay();
    }

    private  void UpdateDisplay()
    {
        Flash();
        DynamicVars["Injury"].BaseValue = Injury[this];
        InvokeDisplayAmountChanged();
    }
}