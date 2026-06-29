using STS2RitsuLib.Scaffolding.Content;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
namespace Diceomancer.Scripts.Cards.Event;

// �����ĸ�����
[RegisterCard(typeof(ColorlessCardPool))]
public class InvincibleStrike() : ModCardTemplate(energyCost, type, rarity, targetType)
{
    // ��������
    private const int energyCost = 3;

    // ��������
    private const CardType type = CardType.Attack;

    // ����ϡ�ж�
    private const CardRarity rarity = CardRarity.Event;

    // Ŀ�����ͣ�AnyEnemy��ʾ������ˣ�?
    private const TargetType targetType = TargetType.RandomEnemy;

    // �Ƿ��ڿ���ͼ������ʾ
    private const bool shouldShowInCardLibrary = true;

    // ���Ӵ��Tag
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];


    // ���ƵĻ�������
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5m, ValueProp.Move), // �˺�
        new RepeatVar(5), // ����
        new("Slippery", 3M)
    ];

    // ������һ�У�ָ����������·����������MyMod/images/cards/Test.png
    // public override string PortraitPath => $"res://MyMod/images/cards/{nameof(Test)}.png";

    // ���ʱ��Ч���߼�?
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState, "base.CombatState");

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue) // ����˺�����ֵ��Դ�ڿ��ƵĻ����˺�����?
            .FromCard(this) // �˺���Դ�����ſ���
            .TargetingRandomOpponents(CombatState) // ���ѡ��Ŀ��?
            .WithHitCount(DynamicVars.Repeat.IntValue) // ��������
            .Execute(choiceContext);

        await PowerCmd.Apply<SlipperyPower>(choiceContext, Owner.Creature, DynamicVars["Slippery"].BaseValue,
            Owner.Creature,
            this);
    }

    // �������Ч���߼�?
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1); // �����������˺�
        DynamicVars.Repeat.UpgradeValueBy(1); // �������Ӷ���
        DynamicVars["Slippery"].UpgradeValueBy(2);
    }
}