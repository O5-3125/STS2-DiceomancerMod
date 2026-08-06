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
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Token;

[RegisterCard(typeof(TokenCardPool))]
public class ElectromagneticCoil() : ModCardTemplate(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(2),
        new PowerVar<HastePower>(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var option1 = Owner.Creature.CombatState.CreateCard<ElectromagneticCoilEnergy>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option1, DynamicVars.Energy.IntValue);
        var option2 = Owner.Creature.CombatState.CreateCard<ElectromagneticCoilHaste>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option2, DynamicVars["HastePower"].IntValue);
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
        EnergyCost.UpgradeBy(-1);
    }
}
