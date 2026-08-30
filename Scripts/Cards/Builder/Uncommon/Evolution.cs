using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Cards.Token.Options;
using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero.Builder;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Builder.Uncommon;

[RegisterCard(typeof(BuilderCardPool))]
public class Evolution() : EvolutionTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self, 2M)
{
    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
        new PowerVar<Excitement>(2),
        new PowerVar<FortifiedPower>(3),
        new PowerVar<HastePower>(2)
    ];
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<Excitement>(),
        HoverTipFactory.FromPower<FortifiedPower>(),
        HoverTipFactory.FromPower<HastePower>(),
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var option1 = Owner.Creature.CombatState.CreateCard<EvolutionExcitement>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option1, DynamicVars["Excitement"].IntValue);
        var option2 = Owner.Creature.CombatState.CreateCard<EvolutionPlating>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option2, DynamicVars["FortifiedPower"].IntValue);
        var option3 = Owner.Creature.CombatState.CreateCard<EvolutionHaste>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option3, DynamicVars["HastePower"].IntValue);
        var options = new List<CardModel> { option1, option2, option3 };

        var cardModel =
            await CardSelectCmd.FromChooseACardScreen(choiceContext, options, Owner, true);
        if (cardModel is not null) await CardCmd.AutoPlay(choiceContext, cardModel.CreateDupe(Owner), null);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Evolution"].UpgradeValueBy(1);
    }
}