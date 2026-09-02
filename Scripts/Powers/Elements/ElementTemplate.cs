using Diceomancer.Scripts.Common.Utils;
using Diceomancer.Scripts.Powers.Berserker;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Cards.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers.Elements;

public abstract class ElementTemplate<TTransform> : Element where TTransform : CardModel
{
    public override async Task AfterRemoved(Creature oldOwner)
    {
        CardModel essence = Owner.CombatState.CreateCard<TTransform>(base.Owner.Player);
        essence.GiveSingleTurnRetain();
        await CardPileCmd.AddGeneratedCardToCombat(essence, PileType.Hand, Owner.Player);

        // await CardPileCmd.AddToCombatAndPreview<TTransform>(Owner, PileType.Hand, 1, null);

        await CheckCycleOfLife();
        await CheckScavenger();
        await CheckVicissitudes();
    }

    private async Task CheckCycleOfLife()
    {
        var cycleOfLifePowerAmount = Owner.GetPowerAmount<CycleOfLifePower>();
        for (var i = 0; i < cycleOfLifePowerAmount; i++)
        {
            await ElementCmd.SummonRandomBasicElement(new ThrowingPlayerChoiceContext(), Owner, Owner, null);
        }
    }

    private async Task CheckScavenger()
    {
        var scavengerPowerAmount = Owner.GetPowerAmount<ScavengerPower>();
        await PlayerCmd.GainEnergy(scavengerPowerAmount, Owner.Player);
    }

    private async Task CheckVicissitudes()
    {
        var vicissitudesPowerAmount = Owner.GetPowerAmount<VicissitudesPower>();

        var elementList = Owner.Powers.Where(ElementCmd.IsElement).ToList();
        if (elementList.Count != 0)
        {
            for (var i = 0; i < vicissitudesPowerAmount; i++)
            {
                var item = Owner.Player.RunState.Rng.Shuffle.NextItem(elementList);
                await PowerCmd.ModifyAmount(new ThrowingPlayerChoiceContext(), item, item.Amount, null, null);
            }
        }
    }
}