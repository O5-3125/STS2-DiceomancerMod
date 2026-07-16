using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Basic;

// 加入角色遗物池
[RegisterRelic(typeof(DiceomancerRelicPool))]
// 加入初始遗物池
[RegisterCharacterStarterRelic(typeof(DiceomancerCharacter))]
public class BuilderRing : ModRelicTemplate
{
    private int _cardsPlayed;

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override string FlashSfx => "event:/sfx/ui/relic_activate_draw";

    public override bool ShowCounter => CombatManager.Instance.IsInProgress;


    // 小图标（原版85x85）
    public override string PackedIconPath => "res://Diceomancer/images/Relics/BuilderRing_big.png";

    // 轮廓图标（原版85x85）
    protected override string PackedIconOutlinePath => "res://Diceomancer/images/Relics/BuilderRing_big.png";

    // 大图标（原版256x256）
    protected override string BigIconPath => "res://Diceomancer/images/Relics/BuilderRing_big.png";

    public override int DisplayAmount => DynamicVars.Cards.IntValue;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3), new EnergyVar(1)];


    private int CardsPlayed
    {
        get => _cardsPlayed;
        set
        {
            AssertMutable();
            _cardsPlayed = value;
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        var intValue = DynamicVars.Cards.IntValue;
        Status = CardsPlayed == intValue - 1 ? RelicStatus.Active : RelicStatus.Normal;


        InvokeDisplayAmountChanged();
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner)
        {
            CardsPlayed++;
            var intValue = DynamicVars.Cards.IntValue;
            if (CombatManager.Instance.IsInProgress && CardsPlayed == intValue)
            {
                Flash();
                await TaskHelper.RunSafely(DoActivateVisuals());
                await CardPileCmd.Draw(context, 2m, Owner);
                await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
            }
        }
    }

    private async Task DoActivateVisuals()
    {
        Flash();
        await Cmd.Wait(1f);
    }

    // 回合结束后
    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext,
        CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (side != Owner.Creature.Side) return Task.CompletedTask;

        CardsPlayed = 0;
        return Task.CompletedTask;
    }

    // 战斗结束后
    public override Task AfterCombatEnd(CombatRoom _)
    {
        CardsPlayed = 0;
        return Task.CompletedTask;
    }
}