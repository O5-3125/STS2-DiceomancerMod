using Diceomancer.Scripts.Powers.NormalityPower;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Orbs.Elements;

[RegisterOrb]
public class FireElementOrb : ElementOrbTemplate
{
    // 被动效果数值，ModifyOrbValue表示是否吃集中等
    public override decimal PassiveVal => ModifyOrbValue(3);

    // 激发效果数值
    public override decimal EvokeVal => ModifyOrbValue(5);

    public override ModOrbValueDisplayMode ValueDisplayMode => ModOrbValueDisplayMode.Contextual;

    public override Color DarkenedColor => new(0.9f, 0.3f, 0.1f);

    public override OrbAssetProfile AssetProfile => new(
        IconPath: "res://Diceomancer/images/Orbs/Element_Fire.png",
        VisualsScenePath: "res://Diceomancer/scenes/Orbs/fire_element_orb.tscn"
    );

    // 触发被动
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        ActivatePassive();
        PlayPassiveSfx();
        var enemy = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
        if (enemy != null)
        {
            await PowerCmd.Apply<BurnPower>(choiceContext, enemy, PassiveVal, Owner.Creature, null);
        }
    }

    // 触发激发，返回受影响的角色
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        PlayEvokeSfx();
        var enemies = CombatState.HittableEnemies;
        ActivateEvoke(enemies.ToArray());
        await PowerCmd.Apply<BurnPower>(playerChoiceContext, enemies, EvokeVal, Owner.Creature, null);
        return enemies;
    }
}