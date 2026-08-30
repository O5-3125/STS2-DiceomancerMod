using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Potions;

[RegisterPotion(typeof(EventPotionPool))]
public class MundanePotion : ModPotionTemplate
{
    // 稀有度
    public override PotionRarity Rarity => PotionRarity.Event;

    // 使用方式，CombatOnly表示只能在战斗中使用。
    public override PotionUsage Usage => PotionUsage.AnyTime;

    // 目标类型
    public override TargetType TargetType => TargetType.Self;

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPotion<MundanePotion>()
    ];

    // 药水图片。不一定非得是png，只要最终能被Godot当成Texture读取即可。
    public override PotionAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Potions/{GetType().Name}.png",
        $"res://Diceomancer/images/Potions/{GetType().Name}.png"
    );

    // 使用时的效果逻辑
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        await PotionCmd.TryToProcure(ModelDb.Potion<MundanePotion>().ToMutable(), Owner);
    }
}