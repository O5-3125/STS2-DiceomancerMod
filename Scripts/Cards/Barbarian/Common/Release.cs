using Diceomancer.Scripts.Cards.Token.Options;
using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero.Barbarian;
using Diceomancer.Scripts.Orbs;
using Diceomancer.Scripts.Orbs.Elements;
using Diceomancer.Scripts.Powers.Berserker;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Barbarian.Common;

[RegisterCard(typeof(BarbarianCardPool))]
public class Release() : ModCardTemplate(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<FuryPower>(3)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<FuryPower>(),
        HoverTipFactory.FromOrb<FireElementOrb>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var option1 = Owner.Creature.CombatState.CreateCard<ReleaseFrenzy>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option1, DynamicVars["FuryPower"].IntValue);
        var option2 = Owner.Creature.CombatState.CreateCard<ReleaseConvert>(Owner);
        var options = new List<CardModel> { option1, option2 };

        var cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, options, Owner, true);
        if (cardModel is not null) await CardCmd.AutoPlay(choiceContext, cardModel.CreateDupe(Owner), null);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}