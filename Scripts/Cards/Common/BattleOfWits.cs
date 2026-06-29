using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Common;

[RegisterCard(typeof(DiceomancerCardPool))]
public class BattleOfWits() :
    ModCardTemplate(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy, true)
{
    // ���ƵĻ������ԣ�����������6���˺���
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6, ValueProp.Move),
        new DynamicVar("kick", 3).WithSharedTooltip("kick")
    ];


    // ���ʱ��Ч���߼�?
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var discardList =
            (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs
                    (base.SelectionScreenPrompt, 0, DynamicVars["kick"].IntValue),
                context: choiceContext, player: base.Owner, filter: null, source: this)).ToList();

        var discardSize = discardList.Count();
        await CardCmd.Discard(choiceContext, discardList);


        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue) // ����˺�����ֵ��Դ�ڿ��ƵĻ����˺�����?
            .FromCard(this) // �˺���Դ�����ſ���
            .WithHitCount(discardSize) // ��������
            .Targeting(cardPlay.Target) // �˺�Ŀ�������ѡ���Ŀ��
            .Execute(choiceContext);
    }

    // �������Ч���߼�?
    protected override void OnUpgrade()
    {
        DynamicVars["kick"].UpgradeValueBy(3);
        DynamicVars.Damage.UpgradeValueBy(4); // ����������4���˺�
    }
}