using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Enchantments;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Common;

[RegisterCard(typeof(DiceomancerCardPool))]
public class QuillSpray() : ModCardTemplate(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
{
    // ��������
    private const int energyCost = 1;

    // ��������
    private const CardType type = CardType.Attack;

    // ����ϡ�ж�
    private const CardRarity rarity = CardRarity.Common;

    // Ŀ�����ͣ�AnyEnemy��ʾ������ˣ�?
    private const TargetType targetType = TargetType.AllEnemies;

    // �Ƿ��ڿ���ͼ������ʾ
    private const bool shouldShowInCardLibrary = true;


    protected override HashSet<CardTag> CanonicalTags =>
    [
        MyTags.Modify.GetModCardTag()
    ];

    // �˺�ֵ
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3, ValueProp.Move),
        new DynamicVar("modify", 3)
            .WithSharedTooltip("modify")
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.IntValue) // ����˺�����ֵ��Դ�ڿ��ƵĻ����˺�����?
            .FromCard(this) // �˺���Դ�����ſ���
            .TargetingAllOpponents(base.CombatState) // �˺�Ŀ�������ѡ���Ŀ��
            .Execute(choiceContext);

        var cardModel = (await CardSelectCmd.FromHand(choiceContext, base.Owner,
            new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, 1),
            (CardModel c) => c.Enchantment == null, this)).FirstOrDefault();
        if (cardModel is { Enchantment: null }) CardCmd.Enchant<Spray>(cardModel, DynamicVars["modify"].IntValue);
    }

    // �������Ч���߼�?
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1); // ����������1���˺�
        DynamicVars["modify"].UpgradeValueBy(1);
        // DynamicVars["Evolution"].UpgradeValueBy(1);
    }
}