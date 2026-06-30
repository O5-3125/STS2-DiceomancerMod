using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Enchantments;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Common;

[RegisterCard(typeof(DiceomancerCardPool))]
public class QuillSpray() : ModCardTemplate(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8, ValueProp.Move),
        new DynamicVar("modify", 3)
            .WithSharedTooltip("modify")
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this)
            .TargetingAllOpponents(base.CombatState)
            .Execute(choiceContext);

        var pile = PileType.Hand.GetPile(base.Owner);
        var cardModel =
            base.Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards.Where((CardModel c) =>
                c.Type == CardType.Attack && c is { Enchantment: Spray }));
        if (cardModel != null)
        {
            switch (cardModel.Enchantment)
            {
                case null:
                    CardCmd.Enchant<Spray>(cardModel, DynamicVars["modify"].IntValue);
                    break;
                case Spray:
                    cardModel.Enchantment.Amount += DynamicVars["modify"].IntValue;
                    break;
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars["modify"].UpgradeValueBy(3);
        // DynamicVars["Evolution"].UpgradeValueBy(1);
    }
}