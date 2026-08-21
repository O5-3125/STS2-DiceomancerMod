using Diceomancer.Scripts.Cards.Token.Options;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Event;

[RegisterCard(typeof(EventCardPool))]
public class Reinforcement() : ModCardTemplate(2, CardType.Skill, CardRarity.Event, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        // CardKeyword.Exhaust
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(16, ValueProp.Move),
        new PowerVar<FortifiedPower>(6)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var option1 = Owner.Creature.CombatState.CreateCard<ReinforcementBlock>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option1, DynamicVars.Block.IntValue);
        var option2 = Owner.Creature.CombatState.CreateCard<ReinforcementPlating>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option2, DynamicVars["FortifiedPower"].IntValue);
        var options = new List<CardModel> { option1, option2 };
        
        foreach (var item in await CardSelectCmd.FromSimpleGrid(choiceContext, options, Owner,
                     new CardSelectorPrefs(SelectionScreenPrompt, 0, 1)))
            await CardCmd.AutoPlay(choiceContext, item.CreateDupe(Owner), null);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}