using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Enchantments;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
namespace Diceomancer.Scripts.Cards.Modify;

[RegisterCard(typeof(TokenCardPool))]
public class ModifyPhasePower()
    : ModCardTemplate(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    // ���ܱ�����
    public override int MaxUpgradeLevel => 0;

    // ������е��������
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override HashSet<CardTag> CanonicalTags =>
    [
        MyTags.Modify.GetModCardTag()
    ];

    // �˺�ֵ
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1)
            .WithSharedTooltip("modify")
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var cardModel = (await CardSelectCmd.FromHand(choiceContext, Owner,
            new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1),
            c => c.Enchantment == null, this)).FirstOrDefault();

        if (cardModel is { Enchantment: null }) CardCmd.Enchant<PhasePower>(cardModel, DynamicVars.Energy.IntValue);
    }
}