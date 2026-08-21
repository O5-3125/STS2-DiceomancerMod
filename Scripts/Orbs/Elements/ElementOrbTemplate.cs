using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Godot;

namespace Diceomancer.Scripts.Orbs.Elements;

public abstract class ElementOrbTemplate : ModOrbTemplate
{
    // 让你不需要手动挂脚本。复制即可。
    protected override Node2D? TryCreateOrbSprite() =>
        RitsuGodotNodeFactories.CreateFromScenePath<Node2D>(AssetProfile.VisualsScenePath!);

    // 回合结束时触发被动
    public override async Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
    {
        await TriggerPassive(choiceContext, null);
    }
}