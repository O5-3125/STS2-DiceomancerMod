using Diceomancer.Scripts.Common.Keywords;
using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero.Barbarian;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Barbarian.Uncommon;

[RegisterCard(typeof(BarbarianCardPool))]
public class ElementalDance() : ModCardTemplate(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [MyKeywords.Wild, MyKeywords.Storm];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await ElementCmd.SummonRandomBasicElement(choiceContext, Owner.Creature, Owner.Creature, this);
    }

    // protected override void OnUpgrade()
    // {
    //     UpgradeBehavior = () => async (choiceContext, cardPlay) =>
    //     {
    //         await ElementCmd.SummonRandomBasicElement(choiceContext, Owner.Creature, Owner.Creature, this);
    //         await ElementCmd.SummonRandomBasicElement(choiceContext, Owner.Creature, Owner.Creature, this);
    //     };
    // }
}
