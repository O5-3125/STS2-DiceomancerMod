using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Potions;

[RegisterPotion(typeof(EventPotionPool))]
public class ForgetMeNot : ModPotionTemplate
{
    // 稀有度
    public override PotionRarity Rarity => PotionRarity.Token;

    // 使用方式，CombatOnly表示只能在战斗中使用。
    public override PotionUsage Usage => PotionUsage.CombatOnly;

    // 目标类型
    public override TargetType TargetType => TargetType.Self;

    // 定义动态变量
    // protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];

    // 这里显示预览卡牌灵魂。或者你也可以添加提示关键词
    // protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromCard<Soul>()];

    // 药水图片。不一定非得是png，只要最终能被Godot当成Texture读取即可。
    public override PotionAssetProfile AssetProfile => new(
        "res://Diceomancer/images/Potions/后悔药.png",
        "res://Diceomancer/images/Potions/后悔药.png"
    );

    // 使用时的效果逻辑
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        IEnumerable<CardModel> cards = PileType.Discard.GetPile(Owner).Cards;

        foreach (var card in cards.ToList())
        {
            if (PileType.Deck.GetPile(Owner).Cards.Contains(card.DeckVersion))
                await CardPileCmd.RemoveFromDeck(card.DeckVersion);

            await CardPileCmd.RemoveFromCombat(card);
        }
    }
}