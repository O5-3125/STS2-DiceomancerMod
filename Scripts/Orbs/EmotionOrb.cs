using Diceomancer.Scripts.Powers.NormalityPower;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Godot;

namespace Diceomancer.Scripts.Orbs;

[RegisterOrb]
public class EmotionOrb : ModOrbTemplate
{
    public override decimal PassiveVal => 0;

    public override decimal EvokeVal => 3;

    public override ModOrbValueDisplayMode ValueDisplayMode => ModOrbValueDisplayMode.SingleEvoke;

    public override Color DarkenedColor => new(0.5f, 0.15f, 0.15f);

    public override OrbAssetProfile AssetProfile => new(
        IconPath: "res://Diceomancer/images/Orbs/ManaRed.png",
        VisualsScenePath: "res://Diceomancer/scenes/Orbs/emotion_orb.tscn"
    );

    protected override Node2D? TryCreateOrbSprite() =>
        RitsuGodotNodeFactories.CreateFromScenePath<Node2D>(AssetProfile.VisualsScenePath!);

    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        PlayEvokeSfx();
        var enemy = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
        if (enemy != null)
        {
            await PowerCmd.Apply<BurnPower>(playerChoiceContext, enemy, EvokeVal, Owner.Creature, null);
        }
        return enemy != null ? [enemy] : [];
    }
}