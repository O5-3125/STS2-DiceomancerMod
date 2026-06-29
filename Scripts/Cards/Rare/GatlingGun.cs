using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Rare;

[RegisterCard(typeof(DiceomancerCardPool))]
public class GatlingGun() : ModCardTemplate(1, CardType.Attack, CardRarity.Rare, TargetType.Self)
{
    // public override IEnumerable<CardKeyword> CanonicalKeywords =>
    // [
    //     MyKeywords.Rebound
    // ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3m, ValueProp.Move),
        new RepeatVar(3)
        // new DynamicVar("Replay",1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(base.CombatState, "base.CombatState");

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue) 
            .FromCard(this)
            .TargetingRandomOpponents(base.CombatState) 
            .WithHitCount(base.DynamicVars.Repeat.IntValue) 
            .Execute(choiceContext);

        BaseReplayCount += 1;
    }
    
    // protected override PileType GetResultPileTypeForCardPlay()
    // {
    //     PileType resultPileType = base.GetResultPileTypeForCardPlay();
    //     if (resultPileType != PileType.Discard) return resultPileType;
    //     return PileType.Hand;
    // }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1); 
    }
}