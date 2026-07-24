using Diceomancer.Scripts.Capabilitys;
using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
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
using STS2RitsuLib.Models.Capabilities;

namespace Diceomancer.Scripts.Cards.Common;

[RegisterCard(typeof(DiceomancerCardPool))]
public class QuillSpray() : ModCardTemplate(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8, ValueProp.Move),
        new DynamicVar("modify", 3)
            .WithSharedTooltip("modify")
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);

        await DamageCmd.Attack(DynamicVars.Damage.IntValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(base.CombatState)
            .Execute(choiceContext);

        var cardModel = (await CardSelectCmd.FromHand(choiceContext, base.Owner,
            new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1),
            null, this)).FirstOrDefault();

        var capability = ModelCapabilityRegistry.Create<SprayCapability>();
        capability.DynamicVars.Damage.BaseValue = DynamicVars["modify"].IntValue;
        cardModel?.AddCapability(capability);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars["modify"].UpgradeValueBy(3);
    }
}