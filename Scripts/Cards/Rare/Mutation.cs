using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Hero;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.CardTags;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Rare;

[RegisterCard(typeof(DiceomancerCardPool))]
public class Mutation() : ModCardTemplate(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    protected override HashSet<CardTag> CanonicalTags => [MyTags.Evolution.GetModCardTag()];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new("Buff", 3),
        new RepeatVar(3),
        new DynamicVar("Evolution", 2M)
            .WithSharedTooltip("evolution")
    ];

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState, "base.CombatState");
        for (var i = 0; i < DynamicVars.Repeat.IntValue; i++)
        {
            await DiceomancerCardCmd.ApplyRandomBuff(choiceContext, Owner, Owner.Creature,
                Owner.Creature, null, DynamicVars["Buff"].IntValue);
        }
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars["Evolution"].UpgradeValueBy(1);
    }
}