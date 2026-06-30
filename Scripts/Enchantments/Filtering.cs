using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Enchantments;

[RegisterEnchantment]
public class Filtering : ModEnchantmentTemplate
{
    // 是否在附魔上显示数值
    public override bool ShowAmount => true;

    // 是否会添加额外的卡牌描述文本
    public override bool HasExtraCardText => true;

    // 像卡牌、遗物、药水等一样，可以使用DynamicVars和ExtraHoverTips
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    // 图标位置。大小1:1就行，原版是64x64
    public override EnchantmentAssetProfile AssetProfile => new(
        "res://icon.svg"
    );

    // 重载这个以改变显示的数字
    // public override int DisplayAmount => DynamicVars.Cards.IntValue;
    
    public override bool CanEnchant(CardModel card)
    {
        return card.Enchantment is Filtering || base.CanEnchant(card);
    }
    // 当附魔的卡牌被打出时调用。
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Card.Owner);
    }

    public override void RecalculateValues()
    {
        DynamicVars.Cards.BaseValue = Amount;
    }
}