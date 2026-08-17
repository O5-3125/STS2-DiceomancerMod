using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero.Berserker;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.Berserker;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Berserker.Common;

[RegisterCard(typeof(BerserkerCardPool))]
public class Burst() : ModCardTemplate(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<RageMaxBonus>(2),
        new PowerVar<FrenzyPower>(4)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        new HoverTip(new LocString("static_hover_tips", "vengeance.title"),
            new LocString("static_hover_tips", "vengeance.description"))
    ];

    protected override bool ShouldGlowGoldInternal =>
        Owner.Creature.GetPowerAmount<Injury>() > 4;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<RageMaxBonus>(choiceContext, Owner.Creature,
            DynamicVars["RageMaxBonus"].IntValue, Owner.Creature, this);

        if (Owner.Creature.GetPowerAmount<Injury>() > 4)
        {
            await PowerCmd.Apply<FrenzyPower>(choiceContext, Owner.Creature,
                DynamicVars["FrenzyPower"].IntValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}