using Diceomancer.Scripts.Powers;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Orbs.Elements;

[RegisterOrb]
public class EarthElementOrb : ElementOrbTemplate
{
    // 被动效果数值，ModifyOrbValue表示是否吃集中等
    public override decimal PassiveVal => ModifyOrbValue(2);

    // 激发效果数值
    public override decimal EvokeVal => ModifyOrbValue(3);

    public override ModOrbValueDisplayMode ValueDisplayMode => ModOrbValueDisplayMode.Contextual;

    public override Color DarkenedColor => new(0.55f, 0.35f, 0.15f);

    public override OrbAssetProfile AssetProfile => new(
        IconPath: "res://Diceomancer/images/Orbs/Element_Earth.png",
        VisualsScenePath: "res://Diceomancer/scenes/Orbs/earth_element_orb.tscn"
    );

    // 触发被动
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        ActivatePassive();
        PlayPassiveSfx();
        var injury = Owner.Creature.GetPower<Injury>();
        if (injury != null)
        {
            await PowerCmd.ModifyAmount(choiceContext, injury, -PassiveVal, Owner.Creature, null);
        }
    }

    // 触发激发，返回受影响的角色
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        PlayEvokeSfx();
        ActivateEvoke([Owner.Creature]);
        await CreatureCmd.Heal(Owner.Creature, EvokeVal);
        return [Owner.Creature];
    }
}