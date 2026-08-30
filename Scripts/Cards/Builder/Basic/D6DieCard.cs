using Diceomancer.Scripts.Cards.Template;
using Diceomancer.Scripts.Enchantments;
using Diceomancer.Scripts.Hero.Builder;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace Diceomancer.Scripts.Cards.Builder.Basic;

[RegisterCard(typeof(BuilderCardPool))]
[RegisterCharacterStarterCard(typeof(Hero.Builder.Builder))]
public class D6DieCard : DieCardTemplate<D6Enchant>
{

}