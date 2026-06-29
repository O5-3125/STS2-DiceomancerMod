using Diceomancer.Scripts.Common;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Enchantments;

[RegisterEnchantment]
public class Phantom : ModEnchantmentTemplate
{
    // 是否在附魔上显示数值
    public override bool ShowAmount => true;

    // 是否会添加额外的卡牌描述文本
    public override bool HasExtraCardText => true;

    // 像卡牌、遗物、药水等一样，可以使用DynamicVars和ExtraHoverTips
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    // 图标位置。大小1:1就行，原版是64x64
    public override EnchantmentAssetProfile AssetProfile => new(
        "res://icon.svg"
    );

    public override bool CanEnchant(CardModel card)
    {
        if (base.CanEnchant(card)) return card.Enchantment is Phantom;

        return false;
    }

    // 当附魔的卡牌被打出时调用。
    public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
    {
        // 打出获得幻影复制
        for (int i = 0; i < Amount; i++)
        {
            var cardModel = cardPlay.Card.CreateClone(); // 获得复制
            cardModel.EnergyCost.AddThisCombat(-1); // 减一费
            cardModel.AddKeyword(CardKeyword.Exhaust); // 消耗
            // cardModel.ClearEnchantmentInternal(); 
            CardCmd.ClearEnchantment(cardPlay.Card); // 移除附魔
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, cardPlay.Card.Owner);
        }
    }

    public override void RecalculateValues()
    {
        DynamicVars.Cards.BaseValue = Amount;
    }
}