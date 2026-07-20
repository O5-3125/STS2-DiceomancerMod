using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Powers;

[RegisterPower]
public sealed class BeaconPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    public override PowerStackType StackType => PowerStackType.Single;


    protected override object InitInternalData() => new Data();


    public override PowerAssetProfile AssetProfile => new(
        $"res://Diceomancer/images/Power/{nameof(BeaconPower)}.png",
        $"res://Diceomancer/images/Power/{nameof(BeaconPower)}.png"
    );

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new StringVar("Card")
    ];

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext,
        ICombatState combatState)
    {
        if (player == Owner.Player)
        {
            var cardModels = GetInternalData<Data>().cardModels;
            foreach (var cardModel in cardModels)
            {
                await CardPileCmd.AddGeneratedCardToCombat(cardModel.CreateClone(), PileType.Hand, Owner.Player);
            }
        }

        await PowerCmd.Remove(this);
    }

    public void SetCardModels(List<CardModel> cardModels)
    {
        List<CardModel> clone = [];
        foreach (var model in cardModels)
        {
            clone.Add(model.CreateClone());

            ((StringVar)DynamicVars["Card"]).StringValue += string.Join('\n', "\n" + "-" + model.Title);
        }

        GetInternalData<Data>().cardModels = clone;
    }

    private class Data
    {
        public List<CardModel>? cardModels;
    }
}