using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Token;

// 薯条：1费衍生状态牌，回复2点血量，消耗
// 由海鸥的塞薯条行动加入玩家的抽牌堆
[RegisterCard(typeof(TokenCardPool))]
public class Fries() : ModCardTemplate(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{


    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HealVar(2)
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.IntValue);
    }

    protected override void OnUpgrade()
    {
    }
}
