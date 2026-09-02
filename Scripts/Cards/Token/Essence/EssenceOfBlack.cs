using Diceomancer.Scripts.Common.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Token.Essence;

[RegisterCard(typeof(TokenCardPool))]
public class EssenceOfBlack()
    : ModCardTemplate(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/Essence.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new("debuff", 1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await RandomPowerCmd.ApplyAllDebuff(choiceContext, cardPlay.Target, Owner.Creature, this,
            DynamicVars["debuff"].IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["debuff"].UpgradeValueBy(2);
    }
}