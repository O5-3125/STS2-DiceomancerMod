using Diceomancer.Scripts.Cards.Token.Options;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.Berserker;
using Diceomancer.Scripts.Powers.NormalityPower;
using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Combat.SecondaryResources;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Berserker.Rare;

[RegisterCard(typeof(BerserkerCardPool))]
public class ControlledAnger() : ModCardTemplate(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [MyKeywords.Limited];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<Injury>(8)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var option1 = Owner.Creature.CombatState.CreateCard<ControlledAngerRage>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option1, 3);
        var option2 = Owner.Creature.CombatState.CreateCard<ControlledAngerDraw>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option2, 4);
        var option3 = Owner.Creature.CombatState.CreateCard<ControlledAngerHeal>(Owner);
        ModifyCardCmd.ModifyCardDynamicVars(option3, DynamicVars["Injury"].IntValue);
        var options = new List<CardModel> { option1, option2, option3 };

        foreach (var item in await CardSelectCmd.FromSimpleGrid(choiceContext, options, Owner,
                     new CardSelectorPrefs(SelectionScreenPrompt, 0, 1)))
        {
            await CardCmd.AutoPlay(choiceContext, item.CreateDupe(Owner), null);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}