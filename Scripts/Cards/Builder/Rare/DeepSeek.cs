using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Cards.Upgrade;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Rare;

[RegisterCard(typeof(BuilderCardPool))]
public class DeepSeek() : UpgradeTemplate<NeuroSama>(0, CardType.Skill, CardRarity.Rare, TargetType.Self,3)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MyKeywords.Limited];
    
    protected override IEnumerable<DynamicVar> OwnCanonicalVars =>
    [
        new CardsVar(5),
        new("Select", 2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var pile = PileType.Draw.GetPile(Owner);
        var topCards = pile.Cards.Take(DynamicVars.Cards.IntValue).ToList();

        var selectCardModels = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(Owner),
            Owner, new CardSelectorPrefs(SelectionScreenPrompt, 0, DynamicVars["Select"].IntValue),
            topCards.Contains)).ToList();

        if (selectCardModels.Count != 0)
        {
            foreach (var selectCardModel in selectCardModels) topCards.Remove(selectCardModel);

            await CardPileCmd.Add(selectCardModels, PileType.Hand);
        }

        await CardCmd.Discard(choiceContext, topCards);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(2);
    }
}