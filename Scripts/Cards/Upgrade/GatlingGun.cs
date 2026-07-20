using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.CardPool;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Upgrade;

[RegisterCard(typeof(UpgradeCardPool))]
public class GatlingGun()
    : ModCardTemplate(1, CardType.Attack, CardRarity.Ancient, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3m, ValueProp.Move),
        new RepeatVar(3)
    ];

    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png" // 卡图
    );
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingRandomOpponents(base.CombatState)
            .WithHitCount(base.DynamicVars.Repeat.IntValue)
            .Execute(choiceContext);

        BaseReplayCount += 1;
    }


    // public override (PileType, CardPilePosition) ModifyCardPlayResultPileTypeAndPosition(CardModel card, bool isAutoPlay,
    //     ResourceInfo resources, PileType pileType, CardPilePosition position)
    // {
    //     return base.ModifyCardPlayResultPileTypeAndPosition(card, isAutoPlay, resources, pileType, position);
    // }

    //
    // protected override (PileType, CardPilePosition) GetResultPileTypeAndPositionForCardPlay()
    // {
    //     var (pileType, item) = base.GetResultPileTypeAndPositionForCardPlay();
    //     if (pileType != PileType.Discard)
    //     {
    //         return (pileType, item);
    //     }
    //
    //     return (PileType.Hand, CardPilePosition.Bottom);
    // }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1);
    }
}