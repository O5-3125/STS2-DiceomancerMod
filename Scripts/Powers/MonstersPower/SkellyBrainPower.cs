using Diceomancer.Scripts.Monsters;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.MonstersPower;

// 模块化脑瓜子：骷髅哥的独有能力
// 当骷髅哥生命条降到一半以下时，如果它有脑瓜子，那么会失去脑瓜子，并将生命条全部回满，同时切换意图。
[RegisterPower]
public class SkellyBrainPower : ModPowerTemplate
{
    // 类型，Buff或Debuff
    public override PowerType Type => PowerType.Buff;

    // 叠加类型，Single表示不可叠加，单纯的标记能力
    public override PowerStackType StackType => PowerStackType.Single;

    // 自定义图标路径。1:1即可。原版游戏大图256x256，小图64x64。
    // 暂时没有图标资源，等有资源后替换为真实图标路径
    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/MonstersPower/{GetType().Name}.png",
        $"res://Diceomancer/images/Power/MonstersPower/{GetType().Name}.png"
    );

    // 生命条发生变化后触发，delta为负代表受到伤害
    public override async Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        if (creature != Owner || delta >= 0m) return;

        // 已经死亡/即将死亡的致命一击不触发（否则会把骷髅哥奶回来）
        if (Owner.IsDead) return;

        // 生命条降到一半以下时触发
        if (Owner.CurrentHp >= Owner.MaxHp / 2m) return;

        // 将生命条全部回满
        await CreatureCmd.Heal(Owner, Owner.MaxHp - Owner.CurrentHp);
        
        // 失去脑瓜子
        await PowerCmd.Remove(this);

        // 当前行动模式切换至甩鞭子，意图变为没脑瓜子的甩鞭子
        if (Owner.Monster is Skelly skelly)
        {
            skelly.SwitchToWhipMode();
            await skelly.RefreshFormAnimation();
        }
    }

    // public override async Task AfterRemoved(Creature oldOwner)
    // {
    //
    // }
}