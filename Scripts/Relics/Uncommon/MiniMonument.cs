using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Uncommon;

[RegisterRelic(typeof(SharedRelicPool))]
public class MiniMonument : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
    public override string PackedIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string BigIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";

    public override async Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(base.Owner.Creature)) return;
        Flash();

        var cardModels =
            (await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(this.SelectionScreenPrompt,
                    0, base.DynamicVars.Cards.IntValue),
                context: choiceContext, player: base.Owner, filter: null, source: this)).ToList();

        foreach (var cardModel in cardModels)
        {
            cardModel.GiveSingleTurnRetain();
        }
    }
}