using System.Reflection;
using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Relics.Basic;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using STS2RitsuLib;
using STS2RitsuLib.Interop;

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

        // 注册初始卡的先古升级
        // 第一个类型参数是你的初始卡，第二个类型参数是被升级成的卡。
        // RitsuLibFramework.RegisterArchaicToothTranscendenceMapping<TestCard, Shiv>();

        // 注册初始遗物的先古升级
        RitsuLibFramework.RegisterTouchOfOrobasRefinementMapping<RedBall, BuilderRing>();
    }
}