using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Rebirth() : ModCardTemplate(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
{
    // ��������
    private const int energyCost = 2;

    // ��������
    private const CardType type = CardType.Attack;

    // ����ϡ�ж�
    private const CardRarity rarity = CardRarity.Uncommon;

    // Ŀ�����ͣ�AnyEnemy��ʾ������ˣ�?
    private const TargetType targetType = TargetType.AnyEnemy;

    // �Ƿ��ڿ���ͼ������ʾ
    private const bool shouldShowInCardLibrary = true;

    protected override HashSet<CardTag> CanonicalTags =>
    [
        MyTags.Evolution.GetModCardTag()
    ];

    // ���ƵĻ������ԣ�����������12���˺���

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(1, ValueProp.Move),
        new DynamicVar("Evolution", 12M)
            .WithSharedTooltip("Evolution")
    ];

    // ������һ�У�ָ����������·����������MyMod/images/cards/Test.png
    // public override string PortraitPath => $"res://MyMod/images/cards/{nameof(Test)}.png";

    // ���ʱ��Ч���߼�?
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue) // ����˺�����ֵ��Դ�ڿ��ƵĻ����˺�����?
            .FromCard(this) // �˺���Դ�����ſ���
            .Targeting(cardPlay.Target) // ���ѡ��Ŀ��?
            .Execute(choiceContext);
    }

    // �������Ч���߼�?
    protected override void OnUpgrade()
    {
        base.EnergyCost.UpgradeBy(-1);
    }
}