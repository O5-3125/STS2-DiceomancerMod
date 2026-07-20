using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Beacon() : ModCardTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardList = PileType.Hand.GetPile(Owner).Cards.ToList();
        if (cardList.Count <= 0) return;

        foreach (var card in cardList)
            await CardCmd.Exhaust(choiceContext, card);
        
        (await PowerCmd.Apply<BeaconPower>(choiceContext,
            base.Owner.Creature, 1m,
            base.Owner.Creature, this))?.SetCardModels(cardList);
    }
    
    protected override void OnUpgrade()
    {
        this.AddKeyword(CardKeyword.Retain);
    }
}