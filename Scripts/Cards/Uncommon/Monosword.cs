using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Uncommon;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Monosword() : ModCardTemplate(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, true)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(9, ValueProp.Unblockable),
        new PowerVar<BleedPower>(9)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        MyKeywords.Phantom // �����Զ���ؼ���?
    ];


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        await DamageCmd.Attack(DynamicVars.Damage.IntValue) // ����˺�����ֵ��Դ�ڿ��ƵĻ����˺�����?
            .FromCard(this) // �˺���Դ�����ſ���
            .Targeting(cardPlay.Target) // ���ѡ��Ŀ��?
            .Execute(choiceContext);

        await PowerCmd.Apply<BleedPower>(choiceContext, cardPlay.Target,
            base.DynamicVars["BleedPower"].BaseValue, base.Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.EnergyCost.UpgradeBy(-1);
    }
}