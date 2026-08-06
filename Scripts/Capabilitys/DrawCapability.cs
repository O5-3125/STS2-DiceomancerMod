using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Models.Capabilities;

namespace Diceomancer.Scripts.Capabilitys;

[RegisterModelCapability]
public class DrawCapability : CardPlayCapability, ICardDescriptionContributor
{
    protected override void OnAttach(CardModel model)
    {
        Log.Info("组件被挂载");
    }

    protected override void OnDetach(CardModel model)
    {
        Log.Info("组件被卸载");
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar("DrawCapability",1)];

    public IEnumerable<CardDescriptionFragment> GetDescriptionFragments(CardDescriptionContext context) =>
        [new(new LocString("enchantments", $"{Id.Entry}.description"))];

    // 当附魔的卡牌被打出时调用。
    protected override async Task OnOwnerCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars["DrawCapability"].IntValue, cardPlay.Card.Owner);
    }
}