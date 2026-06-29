using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Maneuver() : ModCardTemplate(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, true)
{
    protected override bool IsPlayable =>
        base.IsUpgradable || base.Owner.Creature.GetPower<Fatigue>() is null;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(4),
        new("PutBack", 2m)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust,
        MyKeywords.Limited
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);


        var array =
            (await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(base.SelectionScreenPrompt,
                    base.DynamicVars["PutBack"].IntValue,
                    999999),
                context: choiceContext, player: base.Owner, filter: null, source: this)).ToArray();

        if (array.Length != 0) await CardPileCmd.Add(array, PileType.Draw, CardPilePosition.Top);
    }

    protected override void OnUpgrade()
    {
        this.RemoveKeyword(MyKeywords.Limited);
    }
}