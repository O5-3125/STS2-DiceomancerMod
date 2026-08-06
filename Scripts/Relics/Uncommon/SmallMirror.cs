using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Uncommon;

[RegisterRelic(typeof(SharedRelicPool))]
public class SmallMirror : ModRelicTemplate
{
    private int _cardsPlayed;

    private int CardsPlayed
    {
        get => _cardsPlayed;
        set
        {
            AssertMutable();
            _cardsPlayed = value;
            InvokeDisplayAmountChanged();
        }
    }

    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(5)];

    public override bool ShowCounter => CombatManager.Instance.IsInProgress;

    public override int DisplayAmount => _cardsPlayed;

    public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
    {
        if (CardsPlayed != DynamicVars.Cards.IntValue - 1 || card.Owner != Owner) return playCount;

        return playCount + 1;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner && CardsPlayed < DynamicVars.Cards.IntValue) CardsPlayed++;

        if (CardsPlayed == DynamicVars.Cards.IntValue) Flash();

        return Task.CompletedTask;
    }

    // 回合结束后
    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
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

    public override string PackedIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string BigIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
}