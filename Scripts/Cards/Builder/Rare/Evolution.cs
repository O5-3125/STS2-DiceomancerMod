using Diceomancer.Scripts.Cards.Token.Options;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero.Builder;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Rare;

[RegisterCard(typeof(BuilderCardPool))]
public class Evolution() : ModCardTemplate(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override HashSet<CardTag> CanonicalTags =>
    [
        MyTags.Evolution.GetModCardTag()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<Excitement>(2),
        new PowerVar<PlatingPower>(2),
        new PowerVar<HastePower>(2),
        new DynamicVar("Evolution", 2M)
            .WithSharedTooltip("evolution")
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var option1 = Owner.Creature.CombatState.CreateCard<EvolutionExcitement>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option1, DynamicVars["Excitement"].IntValue);
        var option2 = Owner.Creature.CombatState.CreateCard<EvolutionPlating>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option2, DynamicVars["PlatingPower"].IntValue);
        var option3 = Owner.Creature.CombatState.CreateCard<EvolutionHaste>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option3, DynamicVars["HastePower"].IntValue);
        var options = new List<CardModel> { option1, option2, option3 };

        var cardModel =
            await CardSelectCmd.FromChooseACardScreen(choiceContext, options, base.Owner, canSkip: true);
        if (cardModel is not null)
        {
            await CardCmd.AutoPlay(choiceContext, cardModel.CreateDupe(Owner), null);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Evolution"].UpgradeValueBy(1);
    }
}
