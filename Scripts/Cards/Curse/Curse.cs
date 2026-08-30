using Diceomancer.Scripts.Common;
using Diceomancer.Scripts.Common.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Curse;

[RegisterCard(typeof(CurseCardPool))]
public class Curse()
    : ModCardTemplate(0, CardType.Curse, CardRarity.Curse, TargetType.Self)
{
    public override int CanonicalStarCost => 1;

    public override int MaxUpgradeLevel => 0;
    public override bool HasTurnEndInHandEffect => true;

    public override CardAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Cards/{GetType().Name}.png"
    );

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        MyKeywords.Fragile
    ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(1, ValueProp.Unpowered),
        new StarsVar(1)
    ];

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.Damage, this, null);
        await PlayerCmd.GainStars(DynamicVars.Stars.IntValue, Owner);
    }
}