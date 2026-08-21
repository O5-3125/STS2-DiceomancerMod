using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Orbs.Elements;

[RegisterOrb]
public class FusionElementOrb : ElementOrbTemplate
{
    // 暂无效果，有特殊用处
    public override decimal PassiveVal => 0;

    // 暂无效果，有特殊用处
    public override decimal EvokeVal => 0;

    public override ModOrbValueDisplayMode ValueDisplayMode => ModOrbValueDisplayMode.Hidden;

    public override Color DarkenedColor => new(0.6f, 0.6f, 0.6f);

    public override OrbAssetProfile AssetProfile => new(
        IconPath: "res://Diceomancer/images/Orbs/Element__mix.png",
        VisualsScenePath: "res://Diceomancer/scenes/Orbs/fusion_element_orb.tscn"
    );

    // 暂无效果
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
    }

    // 暂无效果
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        return [];
    }
}