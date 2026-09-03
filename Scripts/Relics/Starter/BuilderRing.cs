using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Starter;

[RegisterRelic(typeof(BuilderRelicPool))]
[RegisterCharacterStarterRelic(typeof(Builder))]
public class BuilderRing : ModRelicTemplate
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

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override RelicAssetProfile AssetProfile => new(
        IconPath: $"res://Diceomancer/images/Relics/{GetType().Name}.png",
        IconOutlinePath: $"res://Diceomancer/images/Relics/{GetType().Name}.png",
        BigIconPath: $"res://Diceomancer/images/Relics/{GetType().Name}.png"
    );

    public override bool ShowCounter => CombatManager.Instance.IsInProgress;

    public override int DisplayAmount => _cardsPlayed;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar("Draw", 2),
        new CardsVar("Play", 3),
        new EnergyVar(1)
    ];

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner)
        {
            CardsPlayed++;
        }

        if (CardsPlayed == DynamicVars["Play"].IntValue)
        {
            Flash();
            _ = TriggerEffect(context);
        }

        return Task.CompletedTask;
    }

    private async Task TriggerEffect(PlayerChoiceContext context)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        await CardPileCmd.Draw(context, DynamicVars["Draw"].IntValue, Owner);
    }

    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Creature.Side) return Task.CompletedTask;

        CardsPlayed = 0;
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        CardsPlayed = 0;
        return Task.CompletedTask;
    }
}