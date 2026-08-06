using Diceomancer.Scripts.Cards.Ancient;
using Diceomancer.Scripts.Hero;
using Diceomancer.Scripts.Hero.Builder;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Relics.Ancient;

[RegisterRelic(typeof(SharedRelicPool))]
public class InspirationVoid : ModRelicTemplate
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(5)];
    public override string PackedIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    protected override string BigIconPath => $"res://Diceomancer/images/Relics/{GetType().Name}.png";
    // 通过HoverTipFactory添加各种提示文本
    protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
        HoverTipFactory.FromCardWithCardHoverTips<NullCard>();

    public override async Task AfterObtained()
    {
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 0, DynamicVars.Cards.IntValue)
        {
            Cancelable = false,
            RequireManualConfirmation = true
        };

        var transformations =
            (await CardSelectCmd.FromDeckForTransformation(Owner, prefs,
                c =>
                    new CardTransformation(c, CreateNullCardFromOriginal(c, true)))
            )
            .Select(original =>
                new CardTransformation(original, CreateNullCardFromOriginal(original, false))).ToList();

        await CardCmd.Transform(transformations, Owner.PlayerRng.Transformations);
    }

    private CardModel CreateNullCardFromOriginal(CardModel original, bool forPreview)
    {
        var cardModel = forPreview
            ? ModelDb.Card<NullCard>().ToMutable()
            : Owner.RunState.CreateCard<NullCard>(Owner);
        return cardModel;
    }

    // 小图标（原版85x85）
    // public override string PackedIconPath => $"res://Diceomancer/images/Relics/BuilderRing.png";
    // // 轮廓图标（原版85x85）
    // protected override string PackedIconOutlinePath => $"res://Diceomancer/images/Relics/BuilderRing.png";
    // // 大图标（原版256x256）
    // protected override string BigIconPath => $"res://Diceomancer/images/Relics/BuilderRing_big.png";
}