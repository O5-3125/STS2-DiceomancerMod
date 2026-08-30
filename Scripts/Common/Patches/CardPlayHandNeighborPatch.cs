using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Diceomancer.Scripts.Common.Patches;

public static class ChargedStrikeNeighborTracker
{
    private static readonly Dictionary<CardModel, List<CardModel>> RecordedNeighbors = new();

    public static List<CardModel> GetNeighbors(CardModel played)
    {
        return RecordedNeighbors.TryGetValue(played, out var neighbors)
            ? new List<CardModel>(neighbors)
            : new List<CardModel>();
    }

    public static void Record(CardModel played, List<CardModel> neighbors)
    {
        RecordedNeighbors[played] = neighbors;
    }

    public static void Clear(CardModel played)
    {
        RecordedNeighbors.Remove(played);
    }
}

[HarmonyPatch(typeof(NPlayerHand), "StartCardPlay")]
public static class CardPlayHandNeighborPatch
{
    public static void Prefix(NPlayerHand __instance, NHandCardHolder holder, bool startedViaShortcut)
    {
        if (__instance == null || holder == null) return;

        var cardModel = ((NCardHolder)holder).CardModel;
        if (cardModel == null) return;

        var parent = ((Node)holder).GetParent();
        var holders = parent?.GetChildren(false)
            .OfType<NHandCardHolder>()
            .Where(h => h != null && ((CanvasItem)h).Visible && ((NCardHolder)h).CardModel != null)
            .ToList();
        if (holders == null) return;

        var index = holders.IndexOf(holder);
        if (index < 0) return;

        var neighbors = new List<CardModel>();
        if (index - 1 >= 0)
            neighbors.Add(((NCardHolder)holders[index - 1]).CardModel!);
        if (index + 1 < holders.Count)
            neighbors.Add(((NCardHolder)holders[index + 1]).CardModel!);

        ChargedStrikeNeighborTracker.Record(cardModel, neighbors);
    }
}
