using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;

namespace Diceomancer.Scripts.Cards.Rare;

[RegisterCard(typeof(DiceomancerCardPool))]
public class ButteredCat()
    : UpgradeTemplate<ParadoxEngine>(0, CardType.Skill, CardRarity.Rare, TargetType.Self, 8)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
        new EnergyVar(1),
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var pile = PileType.Hand.GetPile(base.Owner);
        var cardModel = base.Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
        if (cardModel != null)
        {
            if (base.IsUpgraded)
            {
                await CardCmd.Discard(choiceContext, cardModel);
            }
            else
            {
                await CardCmd.Exhaust(choiceContext, cardModel);
            }
        }

        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }
}
