using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Characters;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Godot;

namespace Diceomancer.Scripts.Hero;

[RegisterCharacter]
public class
    DiceomancerCharacter : ModCharacterTemplate<DiceomancerCardPool, DiceomancerRelicPool, DiceomancerPotionPool>
{
    // 角色名称颜色
    public override Color NameColor => new(0.5f, 0.5f, 1f);

    // 能量图标轮廓颜色
    public override Color EnergyLabelOutlineColor => new(0.1f, 0.1f, 1f);

    // 地图绘制颜色
    public override Color MapDrawingColor => new(0.5f, 0.5f, 1f);

    // 人物性别（男女中立）
    public override CharacterGender Gender => CharacterGender.Neutral;

    // 初始血量
    public override int StartingHp => 72;

    public override int StartingGold => 99;


    // 初始充能球栏位
    public override int BaseOrbSlotCount => 0;

    public override CharacterAssetProfile AssetProfile => CharacterAssetProfiles.Merge(
        CharacterAssetProfiles.Ironclad(),
        new CharacterAssetProfile(
            new CharacterSceneAssetSet(
                // 人物模型tscn路径。
                "res://Diceomancer/scenes/Heros/Hero.tscn",
                // 能量表盘tscn路径。
                "res://Diceomancer/scenes/Heros/Hero_energy_counter.tscn",
                // 商店人物场景。
                "res://Diceomancer/scenes/Heros/Hero_merchant.tscn",
                // 篝火休息场景。
                "res://Diceomancer/scenes/Heros/Hero_rest_site.tscn"
            ),
            new CharacterUiAssetSet(
                // 人物头像(图鉴)。
                "res://Diceomancer/images/Hero/icon.png",
                // 人物头像(地图)。
                IconPath: "res://Diceomancer/scenes/Heros/icon.tscn",
                // 人物选择背景。
                CharacterSelectBgPath: "res://Diceomancer/scenes/Heros/Hero_bg.tscn",
                // 人物选择图标。
                CharacterSelectIconPath: "res://Diceomancer/images/Hero/icon.png",
                // 人物选择图标-锁定状态。
                CharacterSelectLockedIconPath: "res://Diceomancer/images/Hero/icon.png"
                // 人物选择过渡动画。
                // CharacterSelectTransitionPath: "res://materials/transitions/ironclad_transition_mat.tres",
                // 地图上的角色标记图标、表情轮盘上的角色头像
                // MapMarkerPath: null
            ),
            new CharacterVfxAssetSet(
                // 卡牌拖尾场景。
                // TrailPath: "res://scenes/vfx/card_trail_ironclad.tscn"
            ),
            Audio: new CharacterAudioAssetSet(
                // 攻击音效
                // AttackSfx: null,
                // 施法音效
                // CastSfx: null,
                // 死亡音效
                // DeathSfx: null,
                // 角色选择音效
                // CharacterSelectSfx: null,
                // 过渡音效
                // CharacterTransitionSfx: "event:/sfx/ui/wipe_ironclad"
            ),
            Multiplayer: new CharacterMultiplayerAssetSet(
                // 多人模式-手指。
                // ArmPointingTexturePath: null,
                // 多人模式剪刀石头布-石头。
                // ArmRockTexturePath: null,
                // 多人模式剪刀石头布-布。
                // ArmPaperTexturePath: null,
                // 多人模式剪刀石头布-剪刀。
                // ArmScissorsTexturePath: null
            )
        )
    ).WithVanillaRelicVisualOverrides([
        new CharacterVanillaRelicVisualOverride(CharacterOwnedVanillaRelicModelId.YummyCookie,
            new RelicAssetProfile("res://Diceomancer/images/Relics/ManaP.png")) // 美味饼干图标路径
    ]);

    // 攻击和施法动画延迟，以对齐动画
    public override float AttackAnimDelay => 0f;
    public override float CastAnimDelay => 0f;

    // 如果你的人物不需要时间线小故事，加上这句。
    public override bool RequiresEpochAndTimeline => false;

    // 自动转换人物场景，让你不需要手动挂脚本。复制即可。
    protected override NCreatureVisuals? TryCreateCreatureVisuals()
    {
        return RitsuGodotNodeFactories.CreateFromScenePath<NCreatureVisuals>(AssetProfile.Scenes!.VisualsPath!);
    }


    // 攻击建筑师的攻击特效列表
    public override List<string> GetArchitectAttackVfx()
    {
        return
        [
            "vfx/vfx_attack_blunt",
            "vfx/vfx_heavy_blunt",
            "vfx/vfx_attack_slash",
            "vfx/vfx_bloody_impact",
            "vfx/vfx_rock_shatter"
        ];
    }
}