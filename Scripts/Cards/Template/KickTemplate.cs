using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Template;

public abstract class KickTemplate(int energyCost, CardType type, CardRarity rarity, TargetType targetType, int kick)
    : ModCardTemplate(energyCost, type, rarity, targetType)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        new HoverTip(new LocString("static_hover_tips", "kick.title"),
            new LocString("static_hover_tips", "kick.description"))
    ];

    protected sealed override IEnumerable<DynamicVar> CanonicalVars =>
        OwnCanonicalVars.Append(new DynamicVar("Kick", kick)
        );

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardModel = cardPlay.Card;
        var player = cardPlay.Player;

        if (cardModel != this || cardModel.IsDupe || cardPlay.IsAutoPlay) return;

        var cardModels =
            (await CardSelectCmd.FromHand(
                prefs: new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 0,
                    DynamicVars["Kick"].IntValue),
                context: choiceContext, player: Owner, filter: null, source: this)).ToList();

        await CardCmd.Discard(choiceContext, cardModels);


        for (int i = 0; i < cardModels.Count; i++)
        {
            await CardCmd.AutoPlay(choiceContext, cardModel.CreateDupe(player), cardPlay.Target);
        }
    }

    protected abstract IEnumerable<DynamicVar> OwnCanonicalVars { get; }
}