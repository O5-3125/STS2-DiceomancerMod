using Diceomancer.Scripts.Common;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interactions.RightClick;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Epidemics;

// [RegisterCard(typeof(UpgradeCardPool))]
public abstract class EpidemicTemplate()
    : ModCardTemplate(0, CardType.Skill, CardRarity.Curse, TargetType.AnyEnemy), IModRightClickableCard
{
    int doom = 1;

    protected override bool IsPlayable => Owner.Creature.GetPowerAmount<DoomPower>() > doom;


    public override IEnumerable<CardKeyword> CanonicalKeywords => [MyKeywords.Epidemic];

    protected override IEnumerable<IHoverTip> AdditionalHoverTips => [HoverTipFactory.FromPower<DoomPower>()];

    public async Task OnRightClick(ModRightClickExecutionContext context)
    {
        
        await CardCmd.Exhaust(context.PlayerChoiceContext, this);
        await PowerCmd.Apply<DoomPower>(context.PlayerChoiceContext, this.Owner.Creature, 3, null, this);
    }
}