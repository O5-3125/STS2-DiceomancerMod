using Diceomancer.Scripts.Cards.Token;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers;
[RegisterPower]

public class TheTimeIsCallingPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    // public override PowerAssetProfile AssetProfile => new(
    //     "res://Diceomancer/images/Power/点燃.png",
    //     "res://Diceomancer/images/Power/点燃.png"
    // );

    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.SummonDynamic, base.DynamicVars.Summon)
    ];


    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        Flash();
        Player? summoner = Owner.Player;
        if (summoner != null)
            await OstyCmd.Summon(choiceContext, summoner, Amount, this);
    }
}