using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Common;

[RegisterCard(typeof(BuilderCardPool))]
public class BattleOfWits() :
    // ModCardTemplate(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    KickTemplate(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, 3)
{
    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
        new DamageVar(6, ValueProp.Move)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }


    protected override void OnUpgrade()
    {
        // DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars["Kick"].BaseValue += 3;
    }
}