using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Powers.NormalityPower;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Curse;


// 加入哪个卡池
[RegisterCard(typeof(CurseCardPool))]
public class Hesitation()
    : ModCardTemplate(1, CardType.Curse, CardRarity.Curse, TargetType.Self)
{
    public override int MaxUpgradeLevel => 0;

    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        MyKeywords.Fragile // 添加自定义关键词
    ];

    // 通过HoverTipFactory添加各种提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<BurdenPower>()
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<BurdenPower>(3)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<BurdenPower>(choiceContext,
            Owner.Creature, DynamicVars["BurdenPower"].IntValue, Owner.Creature, this);
    }
}