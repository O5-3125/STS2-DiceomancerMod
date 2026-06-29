using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Basic;

// 加入角色遗物池
// [RegisterRelic(typeof(DiceomancerRelicPool))]
// 加入初始遗物池
// [RegisterCharacterStarterRelic(typeof(DiceomancerCharacter))]
public class ProRing : ModRelicTemplate
{
// // // // //
    private bool _wasUsedThisTurn;
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override string FlashSfx => "event:/sfx/ui/relic_activate_draw";

    public override bool ShowCounter => false;

    // 小图标（原版85x85）
    public override string PackedIconPath => "res://Diceomancer/images/Relics/ProRing.png";

    // 轮廓图标（原版85x85）
    protected override string PackedIconOutlinePath => "res://Diceomancer/images/Relics/ProRing.png";

    // 大图标（原版256x256）
    protected override string BigIconPath => "res://Diceomancer/images/Relics/ProRing.png";

    private bool WasUsedThisTurn
    {
        get => _wasUsedThisTurn;
        set
        {
            AssertMutable();
            _wasUsedThisTurn = value;
        }
    }


    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Skill && !WasUsedThisTurn)
        {
            Flash();
            await PowerCmd.Apply<FreeAttackPower>(choiceContext, Owner.Creature, 1, null, null);
            // await OrbCmd.Channel<ManaBlue>(choiceContext, base.Owner);
            WasUsedThisTurn = true;
        }
        else if (cardPlay.Card.Type == CardType.Attack && !WasUsedThisTurn)
        {
            Flash();
            // await OrbCmd.Channel<ManaRed>(choiceContext, base.Owner);
            await PowerCmd.Apply<FreeSkillPower>(choiceContext, Owner.Creature, 1, null, null);
            WasUsedThisTurn = true;
        }
    }

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext,
        CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != Owner.Creature.Side) return Task.CompletedTask;

        WasUsedThisTurn = false;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        WasUsedThisTurn = false;
        return Task.CompletedTask;
    }
}