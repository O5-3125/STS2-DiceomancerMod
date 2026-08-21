using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Orbs.Elements;

[RegisterOrb]
public class WaterElementOrb : ElementOrbTemplate
{
    // 被动效果数值，ModifyOrbValue表示是否吃集中等
    public override decimal PassiveVal => ModifyOrbValue(2);

    // 激发效果数值
    public override decimal EvokeVal => ModifyOrbValue(5);

    public override ModOrbValueDisplayMode ValueDisplayMode => ModOrbValueDisplayMode.Contextual;

    public override Color DarkenedColor => new(0.15f, 0.45f, 0.9f);

    public override OrbAssetProfile AssetProfile => new(
        IconPath: "res://Diceomancer/images/Orbs/Element_Water.png",
        VisualsScenePath: "res://Diceomancer/scenes/Orbs/water_element_orb.tscn"
    );

    // 触发被动
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        ActivatePassive();
        PlayPassiveSfx();
        await CreatureCmd.GainBlock(Owner.Creature, PassiveVal, ValueProp.Unpowered, null, false);
    }

    // 触发激发，返回受影响的角色
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        PlayEvokeSfx();
        ActivateEvoke([Owner.Creature]);
        await CreatureCmd.GainBlock(Owner.Creature, EvokeVal, ValueProp.Unpowered, null, false);
        return [Owner.Creature];
    }
}