using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Powers.Berserker;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.Elements;

public abstract class Element : ModPowerTemplate
{
    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/Element/{GetType().Name}.png",
        $"res://Diceomancer/images/Power/Element/{GetType().Name}.png"
    );

    // 类型，Buff或Debuff
    public override PowerType Type => PowerType.Buff;

    // 叠加类型，Counter表示可叠加，Single表示不可叠加
    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;


    /// <summary>
    /// 触发元素效果。
    /// </summary>
    public abstract Task Evoke(PlayerChoiceContext choiceContext);
}