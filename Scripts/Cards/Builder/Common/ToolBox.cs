using Diceomancer.Scripts.Cards.Token.Options;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Common;

[RegisterCard(typeof(BuilderCardPool))]
public class ToolBox() : ModCardTemplate(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6, ValueProp.Move),
        new BlockVar(5, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var option1 = Owner.Creature.CombatState.CreateCard<ToolBoxDamage>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option1, DynamicVars.Damage.IntValue);
        var option2 = Owner.Creature.CombatState.CreateCard<ToolBoxBlock>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option2, DynamicVars.Block.IntValue);
        var options = new List<CardModel> { option1, option2 };

        var cardModel =
            await CardSelectCmd.FromChooseACardScreen(choiceContext, options, base.Owner, canSkip: true);
        if (cardModel is not null)
        {
            await CardCmd.AutoPlay(choiceContext, cardModel.CreateDupe(Owner), null);
        }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(MyKeywords.Bonus);
    }
}
