using Diceomancer.Scripts.Common;
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
public class ChaosDefend() :
    ModCardTemplate(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        // CardKeyword.Exhaust,
        MyKeywords.Chaos6
    ];

    public override bool GainsBlock => true;


    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6m, ValueProp.Move)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }


    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}