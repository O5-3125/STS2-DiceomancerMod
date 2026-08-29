using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Template;

public abstract class RemainTemplate(
    int energyCost,
    CardType type,
    CardRarity rarity,
    TargetType targetType,
    int baseRemain)
    : ModCardTemplate(energyCost, type, rarity, targetType)
{


    private int _currentRemain = baseRemain;

    [SavedProperty]
    private int CurrentRemain
    {
        get => _currentRemain;
        set
        {
            AssertMutable();
            _currentRemain = value;
            DynamicVars["Remain"].BaseValue = _currentRemain;
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Remain", CurrentRemain)
    ];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card != this) return;

        var intValue = DynamicVars["Remain"].IntValue;
        intValue--;
        UpdateFromPlay(intValue);
        (DeckVersion as RemainTemplate)?.UpdateFromPlay(intValue);
        if (intValue <= 0) await CardCmd.Exhaust(choiceContext, this);
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player == Owner && Owner.PlayerCombatState?.TurnNumber == 1 &&
            DynamicVars["Remain"].IntValue <= 0)
            await CardPileCmd.RemoveFromCombat(this);
    }

    public override async Task AfterRestSiteHeal(Player player, bool isMimicked)
    {
        if (player == Owner && CurrentRemain < _currentRemain) UpdateFromPlay(_currentRemain);
    }

    private void UpdateFromPlay(int newRemain)
    {
        CurrentRemain = newRemain;
    }

}