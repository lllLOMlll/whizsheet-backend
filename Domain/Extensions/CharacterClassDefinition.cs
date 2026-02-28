using Whizsheet.Api.Enum;

namespace Whizsheet.Api.Domain.Extensions
{
	public class CharacterClassDefinition
	{
		public int HitDie { get; set; }
		public IReadOnlyCollection<SavingThrowType> SavingThrows { get; init; }
		   = new List<SavingThrowType>();

	}
}
