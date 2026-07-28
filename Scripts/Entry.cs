using System.Reflection;
using Diceomancer.Scripts.Cards.Ancient;
using Diceomancer.Scripts.Cards.Builder.Basic;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.CardPool;
using Diceomancer.Scripts.Relics.Basic;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using STS2RitsuLib;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop;
using STS2RitsuLib.Patching.Core;

namespace Diceomancer.Scripts;

[ModInitializer(nameof(Init))]
public class Entry
{
    // 你的modId
    public const string ModId = "Diceomancer";
    public static readonly Logger Logger = RitsuLibFramework.CreateLogger(ModId);

    public static void Init()
    {
        var assembly = Assembly.GetExecutingAssembly();
        RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, Logger);
        // 自动注册内容
        ModTypeDiscoveryHub.RegisterModAssembly(ModId, assembly);


        // 注册副资源
        // BlackMana.Register();

        // 注册初始卡的先古升级
        // 第一个类型参数是你的初始卡，第二个类型参数是被升级成的卡。
        RitsuLibFramework.RegisterArchaicToothTranscendenceMapping<Pipe, MetalParts>();

        // 注册初始遗物的先古升级
        RitsuLibFramework.RegisterTouchOfOrobasRefinementMapping<BuilderMana, BuilderRing>();

        // 注册卡池
        ModContentRegistry.For(ModId)
            .RegisterCardLibraryCompendiumSharedPoolFilter<UpgradeCardPool>(
                "what_upgrade_card_pool", // ID
                "res://Diceomancer/images/Hero/UpgradeCardPool.png" // 图标位置
                // null // 放置顺序（可选）
            );


// // 所有 TestRelic 实例创建时自动附加 ChargingRelicCapability
//         content.ConfigureDefaultModelCapabilities<TestRelic>(
//             "charge-on-play", // modifier id（同 mod 内唯一）
//             (relic, caps) => caps.Add<Testc>()
//         );
    }
}