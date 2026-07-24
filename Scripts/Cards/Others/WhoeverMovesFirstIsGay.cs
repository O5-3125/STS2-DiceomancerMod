using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Others;

// [RegisterCard(typeof(ColorlessCardPool))]
public class WhoeverMovesFirstIsGay() : ModCardTemplate(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    private const int MaxRemain = 10;

    private int _currentRemain = MaxRemain;

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

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        // new DamageVar(12, ValueProp.Move),
        new IntVar("Remain", CurrentRemain)
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await CreatureCmd.Stun(cardPlay.Target);

        PlayerCmd.EndTurn(base.Owner, canBackOut: false);
    }


    public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card != this) return;

        var intValue = DynamicVars["Remain"].IntValue;
        intValue--;
        UpdateFromPlay(intValue);
        (DeckVersion as WhoeverMovesFirstIsGay)?.UpdateFromPlay(intValue);
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
        if (player == Owner && CurrentRemain < MaxRemain) UpdateFromPlay(MaxRemain);
    }

    private void UpdateFromPlay(int newRemain)
    {
        CurrentRemain = newRemain;
    }


    protected override void OnUpgrade()
    {
        this.RemoveKeyword(CardKeyword.Exhaust);
    }
}