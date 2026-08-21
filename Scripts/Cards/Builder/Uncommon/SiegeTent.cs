using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero.Builder;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Uncommon;

[RegisterCard(typeof(BuilderCardPool))]
public class SiegeTent()
    : UpgradeTemplate<Trebuchet>(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self, 3)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
        new PowerVar<FortifiedPower>(6),
        new BlockVar(10, ValueProp.Move)
    ];

    protected override IEnumerable<IHoverTip> OwnAdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<FortifiedPower>()
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [MyKeywords.Rebound];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

        await PowerCmd.Apply<FortifiedPower>(choiceContext, Owner.Creature,
            DynamicVars["FortifiedPower"].IntValue, Owner.Creature, this);
    }

    // protected override CardLocation GetResultLocationForCardPlay()
    // {
    //     var cardLocation = base.GetResultLocationForCardPlay();
    //     if (cardLocation.pileType != PileType.Discard) return cardLocation;
    //
    //     cardLocation.pileType = PileType.Hand;
    //     cardLocation.position = CardPilePosition.Bottom;
    //
    //     return cardLocation;
    // }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(4);
    }
}