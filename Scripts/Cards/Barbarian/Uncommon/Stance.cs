using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero.Barbarian;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Barbarian.Uncommon;

[RegisterCard(typeof(BarbarianCardPool))]
public class Stance() : ModCardTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new("OrbSlots", 1m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await OrbCmd.AddSlots(Owner, DynamicVars["OrbSlots"].IntValue);

        var orbCount = BarbarianCardUtils.CountEmotionOrbs(Owner);
        if (orbCount > 0) await BarbarianCardUtils.ChannelEmotionOrbs(choiceContext, Owner, orbCount);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}