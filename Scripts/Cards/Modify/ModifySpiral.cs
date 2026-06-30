using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Enchantments;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Modify;

// [RegisterCard(typeof(ModifyCardPool))]
public class ModifySpiral()
    : ModCardTemplate(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override HashSet<CardTag> CanonicalTags =>
    [
        MyTags.Modify.GetModCardTag()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3, ValueProp.Move)
            .WithSharedTooltip("modify")
    ];


    protected override IEnumerable<IHoverTip> AdditionalHoverTips => HoverTipFactory.FromEnchantment<Spiral>();

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var enchantment = ModelDb.Enchantment<Spiral>();

        var cardModel = (await CardSelectCmd.FromHand(choiceContext,
            base.Owner,
            new CardSelectorPrefs(base.SelectionScreenPrompt, 1),
            enchantment.CanEnchant,
            // null,
            this)).FirstOrDefault();

        if (cardModel != null)
        {
            switch (cardModel.Enchantment)
            {
                case null:
                    CardCmd.Enchant<Spiral>(cardModel, DynamicVars.Damage.IntValue);
                    break;
                case Phantom:
                    cardModel.Enchantment.Amount += DynamicVars.Damage.IntValue;
                    break;
            }
        }
    }

    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}