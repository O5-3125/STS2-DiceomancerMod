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
public class DefendBuilder() :
    ModCardTemplate(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    // ��������ƿ��Ի�÷���
    public override bool GainsBlock => true;

    // ���ƵĻ�������
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5m, ValueProp.Move)];

    // ���ʱ��Ч���߼�?
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }

    // �������Ч���߼�?
    protected override void OnUpgrade()
    {
        // DynamicVars.Damage.UpgradeValueBy(4); // ����������4���˺�
        EnergyCost.UpgradeBy(-1); // �ú����?
    }
}