using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Enchantments;
using Diceomancer.Scripts.Hero.Builder;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Basic;

[RegisterCard(typeof(BuilderCardPool))]
public class D20DieCard : DieCardTemplate<D20Enchant>
{

}