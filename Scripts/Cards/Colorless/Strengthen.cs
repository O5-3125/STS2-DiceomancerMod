using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Colorless;
// 变强
// [RegisterCard(typeof(ColorlessCardPool))]
public class Strengthen() : ModCardTemplate(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    // protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    // {
    //     for (var i = 0; i < 3; i++)
    //         await RandomPowerCmd.ApplyRandomBuff(choiceContext, Owner, Owner.Creature, 1, null, null);
    // }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}