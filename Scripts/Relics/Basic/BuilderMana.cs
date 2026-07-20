using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Basic;

// 加入角色遗物池
[RegisterRelic(typeof(DiceomancerRelicPool))]
// 加入初始遗物池
[RegisterCharacterStarterRelic(typeof(DiceomancerCharacter))]
public class BuilderMana : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Common;

    public override string FlashSfx => "event:/sfx/ui/relic_activate_draw";

    // 小图标（原版85x85）
    public override string PackedIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";

    // 轮廓图标（原版85x85）
    protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";

    // 大图标（原版256x256）
    protected override string BigIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<TechPower>(3m)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<TechPower>()
    ];

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (participants.Contains(base.Owner.Creature) && base.Owner.PlayerCombatState.TurnNumber <= 1)
        {
            Flash();
            await PowerCmd.Apply<TechPower>(new ThrowingPlayerChoiceContext(), base.Owner.Creature,
                base.DynamicVars["TechPower"].IntValue, base.Owner.Creature, null);
        }
    }
}