using STS2RitsuLib.Scaffolding.Content;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Cards.Common;

[RegisterCard(typeof(DiceomancerCardPool))]
public class WorkOvertime() : ModCardTemplate(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
{
    // ��������
    private const int energyCost = 0;

    // ��������
    private const CardType type = CardType.Skill;

    // ����ϡ�ж�
    private const CardRarity rarity = CardRarity.Common;

    // Ŀ�����ͣ�AnyEnemy��ʾ������ˣ�?
    private const TargetType targetType = TargetType.Self;

    // �Ƿ��ڿ���ͼ������ʾ
    private const bool shouldShowInCardLibrary = true;

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    // ���ƵĻ������ԣ�����������12���˺���
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(3m),
        new EnergyVar(1),
        new CardsVar(1)
    ];

    // ������һ�У�ָ����������·����������MyMod/images/cards/Test.png
    // public override string PortraitPath => $"res://MyMod/images/cards/{nameof(Test)}.png";

    // ���ʱ��Ч���߼�?
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Damage(choiceContext, base.Owner.Creature, base.DynamicVars.HpLoss.BaseValue,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, base.Owner);
        await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue, base.Owner);
    }

    // �������Ч���߼�?
    protected override void OnUpgrade()
    {
        BaseReplayCount += 1;
    }
}