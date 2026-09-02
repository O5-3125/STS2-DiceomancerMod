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
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Rare;

[RegisterCard(typeof(BuilderCardPool))]
public class NewPerspective() : ModCardTemplate(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!IsUpgraded)
        {
            var drawPile = PileType.Draw.GetPile(Owner).Cards.ToList();
            var handPile = PileType.Hand.GetPile(Owner).Cards.ToList();
            await CardPileCmd.Add(drawPile, PileType.Hand);
            await CardPileCmd.Add(handPile, PileType.Draw);
        }
        else
        {
            var option1 = Owner.Creature.CombatState.CreateCard<NewPerspectiveDraw>(Owner);
            var option2 = Owner.Creature.CombatState.CreateCard<NewPerspectiveDiscard>(Owner);
            var options = new List<CardModel> { option1, option2 };
            var cardModel =
                await CardSelectCmd.FromChooseACardScreen(choiceContext, options, Owner, true);
            if (cardModel is not null) await CardCmd.AutoPlay(choiceContext, cardModel.CreateDupe(Owner), null);
        }
    }

    protected override void OnUpgrade()
    {
    }
}