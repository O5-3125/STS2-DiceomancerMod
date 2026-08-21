using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Orbs.Elements;

[RegisterOrb]
public class ChaosElementOrb : ElementOrbTemplate
{
    // 没有被动效果
    public override decimal PassiveVal => 0;

    // 激发效果数值
    public override decimal EvokeVal => 0;

    public override ModOrbValueDisplayMode ValueDisplayMode => ModOrbValueDisplayMode.Hidden;

    public override Color DarkenedColor => new(0.7f, 0.1f, 0.7f);

    public override OrbAssetProfile AssetProfile => new(
        IconPath: "res://Diceomancer/images/Orbs/Element_Chaos.png",
        VisualsScenePath: "res://Diceomancer/scenes/Orbs/twisted_element_orb.tscn"
    );

    // 没有被动效果
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
    }

    // 触发激发，获得充能球栏位并生成一个扭曲元素
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        PlayEvokeSfx();
        await OrbCmd.AddSlots(Owner, 1);
        await OrbCmd.Channel<ChaosElementOrb>(playerChoiceContext, Owner);
        return [];
    }
}