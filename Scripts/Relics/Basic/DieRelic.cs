using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.Rewards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Rewards;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Basic;

// 骰子遗物基类：卡牌奖励获得"附魔对应骰子"选项，拾起时移除已有的其他骰子遗物。
// 与Driftwood的重掷一致，该选项在一个奖励中只能用一次。
public abstract class DieRelic : ModRelicTemplate
{
    private static readonly FieldInfo OptionsField =
        AccessTools.Field(typeof(NCardRewardSelectionScreen), "_options");

    private CardReward? _lastReward;
    private bool _optionUsed;
    
    protected abstract string OptionKey { get; }


    // public override string PackedIconPath => $"res://Diceomancer/images/Relics/{IconFileName}";
    // protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/{IconFileName}";
    // protected override string BigIconPath => $"res://Diceomancer/images/Relics/{IconFileName}";
    
    public override string PackedIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}";
    protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/{GetType().Name}";
    protected override string BigIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}";

    public override async Task AfterObtained()
    {
        var others = Owner.Relics.Where(r => r is DieRelic && !ReferenceEquals(r, this)).ToList();
        foreach (var other in others)
        {
            await RelicCmd.Remove(other);
        }
        await base.AfterObtained();
    }

    public override bool TryModifyCardRewardAlternatives(Player player, CardReward cardReward, List<CardRewardAlternative> alternatives)
    {
        if (Owner != player) return false;
        if (!ReferenceEquals(_lastReward, cardReward))
        {
            _lastReward = cardReward;
            _optionUsed = false;
        }
        if (_optionUsed) return false;

        alternatives.Add(new CardRewardAlternative(OptionKey, () => OnEnchantReward(cardReward), PostAlternateCardRewardAction.DoNothing));
        return true;
    }

    private Task OnEnchantReward(CardReward cardReward)
    {
        Flash();

        var cards = cardReward.Cards.ToList();
        foreach (CardModel card in cards)
        {
            EnchantCard(card);
        }

        _optionUsed = true;
        RefreshRewardScreen(cardReward);
        return Task.CompletedTask;
    }

    protected abstract void EnchantCard(CardModel card);

    private static void RefreshRewardScreen(CardReward cardReward)
    {
        var screen = NOverlayStack.Instance?.GetNodeOrNull<NCardRewardSelectionScreen>("NCardRewardSelectionScreen");
        if (screen == null) return;

        var options = OptionsField.GetValue(screen) as IReadOnlyList<CardCreationResult>;
        if (options == null) return;

        screen.RefreshOptions(options, CardRewardAlternative.Generate(cardReward));
    }
}
