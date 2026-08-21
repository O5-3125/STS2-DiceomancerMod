using Diceomancer.Scripts.Common.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Orbs.Elements;

[RegisterOrb]
public class DarkElementOrb : ElementOrbTemplate
{
    // 被动效果数值，ModifyOrbValue表示是否吃集中等
    public override decimal PassiveVal => ModifyOrbValue(3);

    // 激发效果数值
    public override decimal EvokeVal => ModifyOrbValue(3);

    public override ModOrbValueDisplayMode ValueDisplayMode => ModOrbValueDisplayMode.Contextual;

    public override Color DarkenedColor => new(0.2f, 0.1f, 0.35f);

    public override OrbAssetProfile AssetProfile => new(
        IconPath: "res://Diceomancer/images/Orbs/Element_Black.png",
        VisualsScenePath: "res://Diceomancer/scenes/Orbs/dark_element_orb.tscn"
    );

    // 触发被动
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        ActivatePassive();
        PlayPassiveSfx();
        var enemy = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
        if (enemy != null)
        {
            await DiceomancerCardCmd.ApplyRandomDebuff(choiceContext, Owner, enemy, Owner.Creature, null,
                PassiveVal);
        }
    }

    // 触发激发，返回受影响的角色
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        PlayEvokeSfx();
        var enemy = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
        if (enemy == null) return [];
        ActivateEvoke([enemy]);
        await DiceomancerCardCmd.ApplyAllDebuff(playerChoiceContext, enemy, Owner.Creature, null, EvokeVal);
        return [enemy];
    }
}