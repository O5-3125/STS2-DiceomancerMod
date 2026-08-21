using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero.Barbarian;
using Diceomancer.Scripts.Orbs.Elements;
using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Barbarian.Basic;

[RegisterCard(typeof(BarbarianCardPool))]
[RegisterCharacterStarterCard(typeof(Hero.Barbarian.Barbarian))]
public class FireOrb() : ModCardTemplate(0, CardType.Attack, CardRarity.Basic, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new RepeatVar(1)
    ];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromOrb<FireElementOrb>()
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await OrbCmd.Channel<FireElementOrb>(choiceContext, Owner);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(MyKeywords.Bonus);
    }
}