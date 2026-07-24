using Diceomancer.Scripts.Cards.Basic;
using Diceomancer.Scripts.Cards.Rare;
using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.CardPool;
using Diceomancer.Scripts.Powers;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;

namespace Diceomancer.Scripts.Cards.Common;

[RegisterCard(typeof(DiceomancerCardPool))]
public sealed class Spike()
    : UpgradeTemplate<SpikeTrap>(2, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, 2)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
        new PowerVar<WeakPower>(2),
        new PowerVar<BleedPower>(8),
    ];

    protected override IEnumerable<IHoverTip> OwnAdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<BleedPower>(),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target,
            DynamicVars.Weak.IntValue, Owner.Creature, this);

        await PowerCmd.Apply<BleedPower>(choiceContext, cardPlay.Target,
            DynamicVars["BleedPower"].IntValue, Owner.Creature, this);
    }
 protected override void OnUpgrade()
    {
        base.DynamicVars.Weak.UpgradeValueBy(1);
        base.DynamicVars["BleedPower"].UpgradeValueBy(2);
    }
}