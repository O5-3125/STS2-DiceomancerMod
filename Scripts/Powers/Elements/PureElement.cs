using System.Collections;
using Diceomancer.Scripts.Cards.Token.Essence;
using Diceomancer.Scripts.Common.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Powers.Elements;

[RegisterPower]
public class PureElement : ElementTemplate<EssenceOfVoid>
{
    public override async Task Evoke(PlayerChoiceContext choiceContext)
    {
        Flash();

        await ElementCmd.SummonRandomBasicElement(choiceContext, Owner, Owner, null);
        await PowerCmd.Decrement(this);
    }

    // 回合结束
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner)) return;
        await Evoke(choiceContext);
    }
}