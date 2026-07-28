using Diceomancer.Scripts.Capabilitys;
using Diceomancer.Scripts.Cards.Template;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;

namespace Diceomancer.Scripts.Cards.Token.Modify;

[RegisterCard(typeof(TokenCardPool))]
public class ModifySpray : ModifyTemplate
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new("modify", 3)
    ];

    protected override void AttachCapability(CardModel cardModel)
    {
        var capability = ModelCapabilityRegistry.Create<SprayCapability>();
        capability.DynamicVars.Damage.BaseValue = DynamicVars["modify"].IntValue;
        cardModel?.AddCapability(capability);
    }
}