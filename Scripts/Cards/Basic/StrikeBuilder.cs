using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
namespace Diceomancer.Scripts.Cards.Basic;

// �����ĸ�����
[RegisterCard(typeof(DiceomancerCardPool))]
// ע���������ʼ��������������������Ҫɾ�����ɡ�?
[RegisterCharacterStarterCard(typeof(DiceomancerCharacter), 4)]
public class StrikeBuilder() :
    ModCardTemplate(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    // ���ƵĻ������ԣ�����������6���˺���
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];

    // ���ʱ��Ч���߼�?
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue) // ����˺�����ֵ��Դ�ڿ��ƵĻ����˺�����?
            .FromCard(this) // �˺���Դ�����ſ���
            .Targeting(cardPlay.Target) // �˺�Ŀ�������ѡ���Ŀ��
            .Execute(choiceContext);
    }

    // �������Ч���߼�?
    protected override void OnUpgrade()
    {
        // DynamicVars.Damage.UpgradeValueBy(4); // ����������4���˺�
        EnergyCost.UpgradeBy(-1); // �ú����?
    }
}