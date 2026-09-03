using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Common.Keywords;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Uncommon;

[RegisterCard(typeof(BuilderCardPool))]
public class CleanSweep() : KickTemplate(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self, 2)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MyKeywords.Bonus,CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
    ];

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}