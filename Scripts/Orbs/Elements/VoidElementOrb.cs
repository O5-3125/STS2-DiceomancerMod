using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Orbs.Elements;

[RegisterOrb]
public class VoidElementOrb : ElementOrbTemplate
{
    private static readonly Type[] BasicElements = [typeof(WaterElementOrb), typeof(FireElementOrb), typeof(EarthElementOrb)];

    // 没有数值
    public override decimal PassiveVal => 0;

    // 没有数值
    public override decimal EvokeVal => 0;

    public override ModOrbValueDisplayMode ValueDisplayMode => ModOrbValueDisplayMode.Hidden;

    public override Color DarkenedColor => new(0.9f, 0.9f, 0.95f);

    public override OrbAssetProfile AssetProfile => new(
        IconPath: "res://Diceomancer/images/Orbs/Element_Void.png",
        VisualsScenePath: "res://Diceomancer/scenes/Orbs/pure_element_orb.tscn"
    );

    // 触发被动，随机生成一个基础元素（水、火、土）
    public override async Task Passive(PlayerChoiceContext choiceContext, Creature? target)
    {
        ActivatePassive();
        PlayPassiveSfx();
        await ChannelBasicElement(choiceContext);
    }

    // 触发激发，生成所有元素各一个（不包括纯粹元素和融合元素）
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
    {
        PlayEvokeSfx();
        await OrbCmd.Channel<WaterElementOrb>(playerChoiceContext, Owner);
        await OrbCmd.Channel<FireElementOrb>(playerChoiceContext, Owner);
        await OrbCmd.Channel<EarthElementOrb>(playerChoiceContext, Owner);
        await OrbCmd.Channel<DarkElementOrb>(playerChoiceContext, Owner);
        await OrbCmd.Channel<ChaosElementOrb>(playerChoiceContext, Owner);
        return [];
    }

    private async Task ChannelBasicElement(PlayerChoiceContext choiceContext)
    {
        var element = Owner.RunState.Rng.CombatCardSelection.NextItem(BasicElements);
        if (element == typeof(WaterElementOrb))
        {
            await OrbCmd.Channel<WaterElementOrb>(choiceContext, Owner);
        }
        else if (element == typeof(FireElementOrb))
        {
            await OrbCmd.Channel<FireElementOrb>(choiceContext, Owner);
        }
        else
        {
            await OrbCmd.Channel<EarthElementOrb>(choiceContext, Owner);
        }
    }
}