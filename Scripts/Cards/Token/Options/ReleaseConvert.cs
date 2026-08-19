using Diceomancer.Scripts.Common.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Token.Options;

[RegisterCard(typeof(TokenCardPool))]
public class ReleaseConvert()
    : ModCardTemplate(-1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await BarbarianCardUtils.ConvertFuryToOrbs(choiceContext, Owner);
    }
}