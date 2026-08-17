using Diceomancer.Scripts.Cards.Token.Options;
using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero.Berserker;
using Diceomancer.Scripts.Powers.Berserker;
using Diceomancer.Scripts.Powers.NormalityPower;
using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Berserker.Uncommon;

[RegisterCard(typeof(BerserkerCardPool))]
public class Bandage() : ModCardTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<InjuryBlock>(1),
        new PowerVar<Injury>(9)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var option1 = Owner.Creature.CombatState.CreateCard<BandageGuard>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option1, DynamicVars["InjuryBlock"].IntValue);
        var option2 = Owner.Creature.CombatState.CreateCard<BandageHeal>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option2, DynamicVars["Injury"].IntValue);
        var options = new List<CardModel> { option1, option2 };

        var cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, options, Owner, canSkip: true);
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