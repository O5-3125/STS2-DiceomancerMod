using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Basic;

[RegisterCard(typeof(BuilderCardPool))]
[RegisterCharacterStarterCard(typeof(Hero.Builder.Builder), 4)]
public class DefendBuilder : ModCardTemplate
{
    public DefendBuilder() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        // this.SecondaryCosts().Set(BlackMana.ManaId, 2);
    }

    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5m, ValueProp.Move)];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }

    protected override void OnUpgrade()
    {
        // DynamicVars.Damage.UpgradeValueBy(4); 
        EnergyCost.UpgradeBy(-1);
    }
}