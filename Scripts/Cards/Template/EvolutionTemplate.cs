using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Common.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Template;

public abstract class EvolutionTemplate(
    int energyCost,
    CardType type,
    CardRarity rarity,
    TargetType targetType,
    decimal evolutionValue)
    : ModCardTemplate(energyCost, type, rarity, targetType)
{
    protected readonly decimal EvolutionValue = evolutionValue;

    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override HashSet<CardTag> CanonicalTags =>
        [MyTags.Evolution.GetModCardTag(), .. OwnCanonicalTags];

    protected virtual IEnumerable<CardTag> OwnCanonicalTags => [];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        OwnCanonicalVars.Append(
            new DynamicVar("Evolution", EvolutionValue)
                .WithSharedTooltip("evolution")
        );

    protected abstract IEnumerable<DynamicVar> OwnCanonicalVars { get; }

    protected override void OnUpgrade()
    {
        DynamicVars["Evolution"].UpgradeValueBy(1);
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card != this) return;

        ModifyCardCmd.ModifyCardDynamicVarsAdditive(this,
            DynamicVars["Evolution"].IntValue, true);
    }
}