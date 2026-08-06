using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;

namespace Diceomancer.Scripts.Capabilitys;

[RegisterModelCapability]
public class SprayCapability : CardPlayCapability, ICardDescriptionContributor
{
    protected override void OnAttach(CardModel model)
    {
        Log.Info("组件被挂载");
    }

    protected override void OnDetach(CardModel model)
    {
        Log.Info("组件被卸载");
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar("SprayCapability",3, ValueProp.Move)
    ];

    public IEnumerable<CardDescriptionFragment> GetDescriptionFragments(CardDescriptionContext context) =>
        [new(new LocString("enchantments", $"{Id.Entry}.description"))];

    // 当附魔的卡牌被打出时调用。
    protected override async Task OnOwnerCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay.Card;

        ArgumentNullException.ThrowIfNull(card.CombatState);

        await DamageCmd.Attack(DynamicVars["SprayCapability"].BaseValue) // 造成伤害，数值来源于卡牌的基础伤害属性
            .FromCard(card, cardPlay) // 伤害来源于这张卡牌
            .TargetingAllOpponents(card.CombatState) // 伤害目标是玩家选择的目标
            .Execute(choiceContext);
    }
}