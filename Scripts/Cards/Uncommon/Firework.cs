using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Cards.Token;
using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Firework()
    : UpgradeTemplate<FireworkArray>(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, 4)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<IHoverTip> OwnAdditionalHoverTips =>
    [
        HoverTipFactory.FromCard<FireworkRocket>(),
    ];

    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
        new CardsVar(3),
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        for (var i = 0; i < DynamicVars.Cards.IntValue; i++)
        {
            CardModel cardModel = Owner.Creature.CombatState.CreateCard<FireworkRocket>(Owner);
            CardCmd.PreviewCardPileAdd(
                await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Draw, Owner, CardPilePosition.Random),
                2.2f);
        }
    }

    protected override void OnUpgrade()
    {
        base.DynamicVars.Cards.UpgradeValueBy(3m);
    }
}