using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;

namespace Diceomancer.Scripts.Common;

[RegisterOwnedCardTag(nameof(Modify))]
[RegisterOwnedCardTag(nameof(Evolution))] // 添加更多就新加这个特性
[RegisterOwnedCardTag(nameof(Upgrade))] // 添加更多就新加这个特性
public class MyTags
{
    public static readonly string Modify = ModContentRegistry.GetQualifiedCardTagId(Entry.ModId, nameof(Modify));

    public static readonly string Evolution = ModContentRegistry.GetQualifiedCardTagId(Entry.ModId, nameof(Evolution));

    public static readonly string Upgrade = ModContentRegistry.GetQualifiedCardTagId(Entry.ModId, nameof(Upgrade));
}