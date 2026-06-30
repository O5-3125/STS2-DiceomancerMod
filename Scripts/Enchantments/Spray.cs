using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Enchantments;

[RegisterEnchantment]
public class Spray : ModEnchantmentTemplate
{
    // 是否显示数值
    public override bool ShowAmount => true;

    // 重载这个以改变显示的数字
    // public override int DisplayAmount => DynamicVars.Cards.IntValue;

    // 是否会添加额外的卡牌描述文本
    public override bool HasExtraCardText => true;

    // 像卡牌、遗物、药水等一样，可以使用DynamicVars和ExtraHoverTips
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7m, ValueProp.Move)];
    // protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromKeyword(CardKeyword.Retain)];

    // 图标位置。大小1:1就行，原版是64x64
    public override EnchantmentAssetProfile AssetProfile => new(
        "res://icon.svg"
    );

    public override bool CanEnchant(CardModel card)
    {
        return card.Enchantment is Spray || base.CanEnchant(card);
    }
    
    // 当附魔的卡牌被打出时调用。
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue) // 造成伤害，数值来源于卡牌的基础伤害属性
            .FromCard(Card) // 伤害来源于这张卡牌
            .TargetingAllOpponents(Card.CombatState) // 伤害目标是玩家选择的目标
            .Execute(choiceContext);
    }

    public override void RecalculateValues()
    {
        DynamicVars.Damage.BaseValue = Amount;
    }
}