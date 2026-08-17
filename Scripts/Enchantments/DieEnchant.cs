using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves.Runs;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Enchantments;

public abstract class DieEnchant : ModEnchantmentTemplate
{
    private int[] _rolledVars = [];

    protected abstract int MaxFace { get; }

    public override EnchantmentAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Enchantment/{GetType().Name}.png"
    );

    // 必须声明为 public：ModelIdSerializationCache.CachePropertiesForType 在具体类型上以
    // Public|NonPublic 旗标枚举属性，基类的私有属性不会被派生类型反射到。
    [SavedProperty]
    public int[] RolledVars
    {
        get => _rolledVars;
        set => _rolledVars = value;
    }

    protected override void OnEnchant()
    {
        var vars = Card.DynamicVars.Values.Where(v => !IsSkippableVar(v)).ToList();

        // 读档路径：卡牌尚未挂载 Owner，且存档里带有之前掷出的数值，直接恢复。
        // 这样 FromSerializable 阶段不会触碰 Card.Owner（此时为 null，会 NRE）。
        if (TryRestoreRolledValues(vars))
        {
            return;
        }

        // 首次附魔时 Card.Owner 已可用，确定性重掷并持久化。
        // 旧存档（没有 RolledVars）且 Owner 为 null 时只能跳过，退回基础数值。
        if (Card.Owner == null)
        {
            return;
        }

        _rolledVars = new int[vars.Count * 2];
        for (var i = 0; i < vars.Count; i++)
        {
            var rng = new Rng(Card.Owner, Id, StringHelper.GetDeterministicHashCode(vars[i].Name));
            var value = rng.NextInt(1, MaxFace + 1);
            vars[i].BaseValue = value;
            _rolledVars[i * 2] = (int)StringHelper.GetDeterministicHashCode(vars[i].Name);
            _rolledVars[i * 2 + 1] = value;
        }
    }

    private bool TryRestoreRolledValues(List<DynamicVar> vars)
    {
        if (_rolledVars.Length == 0 || _rolledVars.Length % 2 != 0)
        {
            return false;
        }

        var values = new Dictionary<int, int>();
        for (var i = 0; i < _rolledVars.Length; i += 2)
        {
            values[_rolledVars[i]] = _rolledVars[i + 1];
        }

        var restoredAny = false;
        foreach (var dvar in vars)
        {
            var nameHash = (int)StringHelper.GetDeterministicHashCode(dvar.Name);
            if (values.TryGetValue(nameHash, out var value))
            {
                dvar.BaseValue = value;
                restoredAny = true;
            }
        }
        return restoredAny;
    }

    private static bool IsSkippableVar(DynamicVar v)
    {
        return v.GetType().Name.Contains("Calculated", StringComparison.OrdinalIgnoreCase);
    }
}
