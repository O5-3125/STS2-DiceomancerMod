using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Token;

// 烫手炸弹！：1费衍生攻击牌，造成6点伤害。
// 如果回合结束还在手牌中，对玩家造成6点伤害。消耗，虚无。
[RegisterCard(typeof(TokenCardPool))]
public class HotBomb() : ModCardTemplate(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
{
    private const int Damage = 6;

    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Ethereal];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(Damage, ValueProp.Move)
    ];

    // 回合结束还在手牌中：对玩家造成6点伤害（虚无会随后消耗掉这张牌）
    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(Damage)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.Damage(choiceContext, Owner.Creature, Damage,
            ValueProp.Unpowered | ValueProp.Move, this, null);
    }

    protected override void OnUpgrade()
    {
    }
}
