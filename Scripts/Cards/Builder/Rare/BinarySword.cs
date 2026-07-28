using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Rare;

[RegisterCard(typeof(BuilderCardPool))]
public class BinarySword() : ModCardTemplate(3, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords => [MyKeywords.Chaos];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("A", 1)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;

        var enemy = cardPlay.Target;

        await CreatureCmd.SetMaxHp(enemy, ReplaceDigitsMath(enemy.MaxHp, DynamicVars["A"].IntValue));
        await CreatureCmd.SetCurrentHp(enemy, ReplaceDigitsMath(enemy.CurrentHp, DynamicVars["A"].IntValue));

        var powerModels = enemy.Powers.ToList();
        foreach (var powerModel in powerModels.Where(powerModel => powerModel.StackType == PowerStackType.Counter))
            powerModel.SetAmount(ReplaceDigitsMath(powerModel.Amount, DynamicVars["A"].IntValue));
    }

    private static int ReplaceDigitsMath(int num, int a)
    {
        if (a == 0 || num == 0) return 0;
        if (a > 9) a = 9;

        var result = 0;
        var multiplier = 1;

        // 处理每一位，但注意这里是从低位到高位构造
        var temp = num;
        while (temp > 0)
        {
            result += a * multiplier;
            multiplier *= 10;
            temp /= 10;
        }

        return result;
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}