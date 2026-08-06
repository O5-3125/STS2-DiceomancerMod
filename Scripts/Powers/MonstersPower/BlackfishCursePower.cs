using Diceomancer.Scripts.Cards.Token;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.MonstersPower;

// 黑鱼诅咒被动：玩家除第一回合外每回合开始选择诅咒。
// Y初始为1，每两回合增加1（上限4）；从14种诅咒中抽X种让玩家选Y项，X=2Y（至少3，至多8）。
[RegisterPower]
public class BlackfishCursePower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/MonstersPower/全知.png",
        $"res://Diceomancer/images/Power/MonstersPower/全知.png"
    );

    // 玩家回合开始时选择诅咒（第1回合不选）
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        var round = player.Creature.CombatState.RoundNumber;
        if (round < 2) return;

        // Y初始为1，每两回合增加1，上限4；X = clamp(2Y, 3, 8)
        var y = Math.Min(1 + (round - 2) / 2, 4);
        var curseNum = Math.Clamp(2 * y, 3, 8);

        var curseList = RandomCurses(player, curseNum);

        

        // 玩家选择Y项
        var prefs = new CardSelectorPrefs(
            new LocString("cards", "DICEOMANCER_CARD_BLACKFISH_CURSE.prompt"), y);

        var chosen = (await CardSelectCmd.FromSimpleGrid(choiceContext, curseList, player, prefs)).ToList();


        // 执行被选中的诅咒
        foreach (var card in chosen)
        {
            if (card is IBlackfishCurse curse)
            {
                await curse.OnChosen();
            }
        }
    }

    private static List<CardModel> RandomCurses(Player player, int num)
    {
        // 从14种诅咒中随机抽X种
        var pool = AllCurses;
        var options = new List<CardModel>();


        for (var i = 0; i < num && pool.Count > 0; i++)
        {
            var idx = player.RunState.Rng.CombatCardSelection.NextItem(pool);
            var cardModel = player.Creature.CombatState.CreateCard((CardModel)idx, player);

            options.Add(cardModel);
            pool.Remove(idx);
        }

        return options;
    }


    private static List<CardModel> AllCurses =>
    [
        ModelDb.Card<CurseSelfDamage>(),
        ModelDb.Card<CurseLoseHp>(),
        ModelDb.Card<CurseLoseMaxHp>(),
        ModelDb.Card<CurseDiscardCards>(),
        ModelDb.Card<CurseFrail>(),
        ModelDb.Card<CurseWeak>(),
        ModelDb.Card<CurseVulnerable>(),
        ModelDb.Card<CursePanic>(),
        ModelDb.Card<CurseSlow>(),
        ModelDb.Card<CurseBlind>(),
        ModelDb.Card<CurseVoidCards>(),
        ModelDb.Card<CurseSlimedCards>(),
        ModelDb.Card<CurseEnemyStrength>(),
        ModelDb.Card<CurseEnemyFortify>(),
    ];
}