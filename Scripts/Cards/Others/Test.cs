using Diceomancer.Scripts.Hero.Builder;
using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Others;

// [RegisterCard(typeof(TokenCardPool))]
public class Test() : ModCardTemplate(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await StunForTwoTurns(cardPlay.Target);
    }

    // 击晕两回合
    private static async Task StunForTwoTurns(Creature target)
    {
        var nextMoveId = target.Monster!.MoveStateMachine
            .StateLog.Last()
            .GetNextState(target, target.Monster.RunRng.MonsterAi);

        await CreatureCmd.Stun(
            target,
            _ => CreatureCmd.Stun(
                target,
                _ => Task.CompletedTask,
                nextMoveId),
            nextMoveId);
    }

    // protected override void OnUpgrade()
    // {
    //     EnergyCost.UpgradeBy(-1);
    // }
}