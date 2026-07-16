using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Basic;

// [RegisterCard(typeof(DiceomancerCardPool))]
// [RegisterCharacterStarterCard(typeof(DiceomancerCharacter))]
public class TestSkill()
    : ModCardTemplate(0, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    private CardType ThisType { get; set; } = CardType.Skill;

    public override CardType Type => ThisType;

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    public override bool GainsBlock => true;


    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(5m, ValueProp.Move)
    ];

    // protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    // {
    //     // IEnumerable<CardDrawnEntry> cardDrawnEntries = CombatManager.Instance.History.Entries.OfType<CardDrawnEntry>();
    //     
    //     this.ThisType = this.ThisType switch
    //     {
    //         // CardType.None => CardType.Attack,
    //         CardType.Skill => CardType.Attack,
    //         CardType.Attack => CardType.Skill,
    //         _ => this.ThisType
    //     };
    // }
}