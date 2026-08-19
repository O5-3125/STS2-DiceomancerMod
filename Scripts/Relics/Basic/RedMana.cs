using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.Barbarian;
using Diceomancer.Scripts.Hero.Builder;
using Diceomancer.Scripts.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Basic;

[RegisterRelic(typeof(BarbarianRelicPool))]
[RegisterCharacterStarterRelic(typeof(Barbarian))]
public class RedMana : ModRelicTemplate, ICardOnPlayHookListener
{
    public override RelicRarity Rarity => RelicRarity.Common;

    // 小图标（原版85x85）
    public override string PackedIconPath => "res://Diceomancer/images/Relics/RedBall.png";

    // 轮廓图标（原版85x85）
    protected override string PackedIconOutlinePath => "res://Diceomancer/images/Relics/RedBall.png";

    // 大图标（原版256x256）
    protected override string BigIconPath => "res://Diceomancer/images/Relics/RedBall.png";

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.FromPower<Injury>()
    ];

    public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner.Creature || props.HasFlag(ValueProp.Unblockable) || amount == 0) return amount;

        Flash();
        PowerCmd.Apply<Injury>(new ThrowingPlayerChoiceContext(), target, amount, null, null);
        return 0m;
    }
}