using Godot;
using STS2RitsuLib;
using STS2RitsuLib.Combat.SecondaryResources;

namespace Diceomancer.Scripts.Hero;

public static class BlackMana
{
    public static SecondaryResourceDefinition ManaDefinition { get; private set; } = null!;
    public static string ManaId { get; private set; } = string.Empty;

    public static void Register()
    {
        var registry = RitsuLibFramework.GetSecondaryResourceRegistry(Entry.ModId);

        ManaDefinition = registry.Register("black", new SecondaryResourceDefinition(
            defaultAmount: 0,
            baseMaxAmount: 3,
            turnStartPolicy: SecondaryResourceTurnStartPolicy.None,
            persistencePolicy: SecondaryResourcePersistencePolicy.Combat,
            smallIconPath: "res://Diceomancer/images/Energy/ManaBlack.png",
            largeIconPath: "res://Diceomancer/images/Energy/ManaBlack.png"
        ));
        ManaId = ManaDefinition.Id;

        // 战斗计数器。使用的图标就是你注册时提供的图标
        registry.RegisterCombatUi(
            "black_combat_counter",
            parent =>
            {
                var row = NSecondaryResourceCounter.Create(ManaDefinition, new SecondaryResourceCounterStyle
                {
                    FontSize = 32,
                    PositiveColor = Colors.Cyan,
                    FormatAmount = (amount, max) => amount.ToString(),
                    IconStyle = SecondaryResourceIconStyle.Default with
                    {
                        Size = new Vector2(80, 80),
                        HoverTip = SecondaryResourceHoverTipStyle.Default,
                    },
                });
                // 自由指定位置。例如这里我们找到能量计数器的位置，放在它旁边
                var energyCounter = parent.GetNode<Control>("%EnergyCounterContainer");
                row.Position = energyCounter.Position + new Vector2(120, -120);
                return row;
            },
            ctx => ctx.Node.Bind(ctx.Player)
        );

        // 卡牌面上的次级资源费用显示。使用的图标就是你注册时提供的图标
        registry.RegisterCardUi(
            "black_card_ui",
            parent =>
            {
                var ui = NSecondaryResourceCardCostUi.Create(ManaId, new SecondaryResourceCardCostUiStyle
                {
                    IconSize = new Vector2(48, 48),
                    FontSize = 24,
                });
                // 自由指定位置。例如这里我们找到能量图标的位置，放在它旁边
                var energyIcon = parent.GetNode<TextureRect>("%EnergyIcon");
                ui.Position = energyIcon.Position + new Vector2(0, 80);
                return ui;
            },
            ctx => ctx.Node.Refresh(ctx));
    }
}