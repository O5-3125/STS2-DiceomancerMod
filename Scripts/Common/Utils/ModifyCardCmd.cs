using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Models.Capabilities;

namespace Diceomancer.Scripts.Common.Utils;

public static class ModifyCardCmd
{
    public static  void   DiceRollCardList(Player player, IEnumerable<CardModel> cardModels)
    {
        foreach (var model in cardModels)
        {
            DiceRollCard(player, model);
        }
    }

    public static void DiceRollCard(Player player, CardModel cardModel)
    {
        var keyList = cardModel.DynamicVars.Keys;
        foreach (var key in keyList)
            cardModel.DynamicVars[key].BaseValue =
                RandomCmd.CheckDiceRoll(player, (int)cardModel.DynamicVars[key].BaseValue);

        // if (cardModel.BaseReplayCount != 0)
        // {
        //     cardModel.BaseReplayCount = amount;
        // }

        var allCapabilities = cardModel.Capabilities().All;
        foreach (var capability in allCapabilities)
        {
            if (capability is not ModelCapability modelCap) continue;
            var capKeys = modelCap.DynamicVars.Keys.ToList();
            foreach (var capKey in capKeys)
                modelCap.DynamicVars[capKey].BaseValue =
                    RandomCmd.CheckDiceRoll(player, (int)modelCap.DynamicVars[capKey].BaseValue);
        }
    }


    public static void ModifyCardListDynamicVars(IEnumerable<CardModel> cardModels, int amount)
    {
        foreach (var model in cardModels)
        {
            ModifyCardDynamicVars(model, amount);
        }
    }

    public static void ModifyCardListDynamicVarsAdditive(IEnumerable<CardModel> cardModels, int amount)
    {
        foreach (var model in cardModels)
        {
            ModifyCardDynamicVarsAdditive(model, amount);
        }
    }

    public static void ModifyCardListDynamicVarsMultiplicative(IEnumerable<CardModel> cardModels, int amount)
    {
        foreach (var model in cardModels)
        {
            ModifyCardDynamicVarMultiplicative(model, amount);
        }
    }

    public static void ModifyCardDynamicVars(CardModel cardModel, int amount)
    {
        var keyList = cardModel.DynamicVars.Keys;
        foreach (var key in keyList)
            cardModel.DynamicVars[key].BaseValue = amount;

        if (cardModel.BaseReplayCount != 0)
        {
            cardModel.BaseReplayCount = amount;
        }

        var allCapabilities = cardModel.Capabilities().All;
        foreach (var capability in allCapabilities)
        {
            if (capability is ModelCapability modelCap)
            {
                var capKeys = modelCap.DynamicVars.Keys.ToList();
                foreach (var capKey in capKeys)
                    modelCap.DynamicVars[capKey].BaseValue = amount;
            }
        }
    }

    public static void ModifyCardDynamicVarsAdditive(CardModel cardModel, int amount, bool isEvolution = false)
    {
        var keyList = cardModel.DynamicVars.Keys;
        foreach (var key in keyList)
        {
            if (isEvolution && key == "Evolution") continue;
            cardModel.DynamicVars[key].BaseValue += amount;
        }

        if (cardModel.BaseReplayCount != 0)
        {
            cardModel.BaseReplayCount += amount;

            if (cardModel.BaseReplayCount < 0)
            {
                cardModel.BaseReplayCount = 0;
            }
        }

        var allCapabilities = cardModel.Capabilities().All;
        foreach (var capability in allCapabilities)
        {
            if (capability is ModelCapability modelCap)
            {
                var capKeys = modelCap.DynamicVars.Keys.ToList();
                foreach (var capKey in capKeys)
                    modelCap.DynamicVars[capKey].BaseValue += amount;
            }
        }
    }

    public static void ModifyCardDynamicVarMultiplicative(CardModel cardModel, int amount)
    {
        var keyList = cardModel.DynamicVars.Keys;
        foreach (var key in keyList)
        {
            cardModel.DynamicVars[key].BaseValue *= amount;
        }

        if (cardModel.BaseReplayCount != 0)
        {
            cardModel.BaseReplayCount *= amount;
        }


        var allCapabilities = cardModel.Capabilities().All;
        foreach (var capability in allCapabilities)
        {
            if (capability is ModelCapability modelCap)
            {
                var capKeys = modelCap.DynamicVars.Keys.ToList();
                foreach (var capKey in capKeys)
                    modelCap.DynamicVars[capKey].BaseValue *= amount;
            }
        }
    }
}