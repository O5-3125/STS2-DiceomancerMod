using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Common.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Basic;

[RegisterCard(typeof(TokenCardPool))]
public class TestSkill()
    : ModCardTemplate(0, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        MyKeywords.Storm
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await BarbarianCardUtils.ChannelEmotionOrbs(choiceContext, Owner, 1);
    }
}