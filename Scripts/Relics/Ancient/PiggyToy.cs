using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.Builder;
using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Ancient;

[RegisterRelic(typeof(SharedRelicPool))]
public class PiggyToy : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override string PackedIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string BigIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<ThickSkin>(4m)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<ThickSkin>()
    ];

    public override async Task BeforeCombatStart()
    {
        Flash();
        await PowerCmd.Apply<ThickSkin>(new ThrowingPlayerChoiceContext(), Owner.Creature,
            DynamicVars["ThickSkin"].BaseValue, Owner.Creature, null);
    }
}
