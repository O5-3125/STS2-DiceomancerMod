using Diceomancer.Scripts.Cards.Builder.Common;
using Diceomancer.Scripts.Powers.Elements;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Diceomancer.Scripts.Common.Utils;

public static class ElementCmd
{
    private static readonly ElementKind[] BasicElementKind =
    [
        ElementKind.FireElement,
        ElementKind.WaterElement,
        ElementKind.EarthElement
    ];

    public static bool IsElement(PowerModel powerModel)
    {
        return powerModel is FireElement or WaterElement or EarthElement
            or BlackElement or SpaceFireElement or SteamTankElement;
    }

    public static bool IsBasicElement(PowerModel powerModel)
    {
        return powerModel is FireElement or WaterElement or EarthElement;
    }

    // FireElement 火元素
    public static async Task SummonFireElement
    (PlayerChoiceContext choiceContext, Creature target, Creature applier, CardModel? cardSource,
        decimal amount = 4, decimal damage = 4)
    {
        var fireElement =
            await PowerCmd.Apply<FireElement>(choiceContext, target, amount, applier, cardSource);
        if (fireElement != null) fireElement.DynamicVars.Damage.BaseValue = damage;
    }

    // SpaceFireElement 星际火元素
    public static async Task SummonSpaceFireElement
    (PlayerChoiceContext choiceContext, Creature target, Creature applier, CardModel? cardSource,
        decimal amount = 20, decimal damage = 6, decimal repeat = 6)
    {
        var spaceFireElement =
            await PowerCmd.Apply<SpaceFireElement>(choiceContext, target, amount, applier, cardSource);
        if (spaceFireElement != null)
        {
            spaceFireElement.DynamicVars.Damage.BaseValue = damage;
            spaceFireElement.DynamicVars.Repeat.BaseValue = repeat;
        }
    }

    // WaterElement 水元素
    public static async Task SummonWaterElement
    (PlayerChoiceContext choiceContext, Creature target, Creature applier, CardModel? cardSource,
        decimal amount = 3, decimal block = 3)
    {
        var waterElement =
            await PowerCmd.Apply<WaterElement>(choiceContext, target, amount, applier, cardSource);
        if (waterElement != null) waterElement.DynamicVars.Block.BaseValue = block;
    }

    // EarthElement 土元素
    public static async Task SummonEarthElement
    (PlayerChoiceContext choiceContext, Creature target, Creature applier, CardModel? cardSource,
        decimal amount = 3, decimal energy = 1)
    {
        var earthElement =
            await PowerCmd.Apply<EarthElement>(choiceContext, target, amount, applier, cardSource);
        if (earthElement != null) earthElement.DynamicVars.Energy.BaseValue = energy;
    }

    // BlackElement 黯元素
    public static async Task SummonBlackElement
    (PlayerChoiceContext choiceContext, Creature target, Creature applier, CardModel? cardSource,
        decimal amount = 5, decimal buff = 3)
    {
        var blackElement =
            await PowerCmd.Apply<BlackElement>(choiceContext, target, amount, applier, cardSource);
        if (blackElement != null) blackElement.DynamicVars["Buff"].BaseValue = buff;
    }

    // SteamTankElement 蒸汽坦克
    public static async Task SummonSteamTankElement
    (PlayerChoiceContext choiceContext, Creature target, Creature applier, CardModel? cardSource,
        decimal amount = 6, decimal damage = 20, decimal block = 10)
    {
        var steamTankElement =
            await PowerCmd.Apply<SteamTankElement>(choiceContext, target, amount, applier, cardSource);
        if (steamTankElement != null)
        {
            steamTankElement.DynamicVars.Damage.BaseValue = damage;
            steamTankElement.DynamicVars.Block.BaseValue = block;
        }
    }

    // PureElement 纯粹元素
    public static async Task SummonPureElement
    (PlayerChoiceContext choiceContext, Creature target, Creature applier, CardModel? cardSource,
        decimal amount = 4)
    {
        await PowerCmd.Apply<PureElement>(choiceContext, target, amount, applier, cardSource);
    }


    // SummonRandomBasicElement 随机召唤基础元素（水、火、土）
    public static async Task SummonRandomBasicElement
        (PlayerChoiceContext choiceContext, Creature target, Creature applier, CardModel? cardSource)
    {
        var elementType = applier.Player.RunState.Rng.Shuffle.NextItem(BasicElementKind);

        await SummonElement(choiceContext, target, applier, cardSource, elementType);
    }

    // SummonRandomBasicElement 召唤所有基础元素（水、火、土）
    public static async Task SummonAllBasicElement
        (PlayerChoiceContext choiceContext, Creature target, Creature applier, CardModel? cardSource)
    {
        foreach (var kind in BasicElementKind)
            await SummonElement(choiceContext, target, applier, cardSource, kind);
    }

    public static async Task SummonElement
    (PlayerChoiceContext choiceContext, Creature target, Creature applier, CardModel? cardSource,
        ElementKind elementType)
    {
        switch (elementType)
        {
            case ElementKind.FireElement:
                await SummonFireElement(choiceContext, target, applier, cardSource);
                break;
            case ElementKind.SpaceFireElement:
                await SummonSpaceFireElement(choiceContext, target, applier, cardSource);
                break;
            case ElementKind.WaterElement:
                await SummonWaterElement(choiceContext, target, applier, cardSource);
                break;
            case ElementKind.EarthElement:
                await SummonEarthElement(choiceContext, target, applier, cardSource);
                break;
            case ElementKind.BlackElement:
                await SummonBlackElement(choiceContext, target, applier, cardSource);
                break;
            case ElementKind.SteamTankElement:
                await SummonSteamTankElement(choiceContext, target, applier, cardSource);
                break;
            default:
                break;
        }
    }

    public static async Task CopyElement(PlayerChoiceContext choiceContext, Creature creature, CardModel? cardSource)
    {
        var elements = creature.Powers.Where(IsElement).ToList();

        foreach (var element in elements)
        {
            var copy = (PowerModel)element.ClonePreservingMutability();
            var copyAmount = copy.Amount;

            await PowerCmd.Apply(choiceContext, copy, creature, copyAmount, creature, cardSource);
        }
    }

    public static async Task Evoke(PlayerChoiceContext choiceContext, Element element)
    {
        await element.Evoke(choiceContext);
    }

    public static async Task EvokeToDie(PlayerChoiceContext choiceContext, Element element)
    {
        var amount = element.Amount;
        for (int i = 0; i <= amount; i++) await Evoke(choiceContext, element);
    }
}