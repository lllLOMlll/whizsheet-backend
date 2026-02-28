using Microsoft.OpenApi.Models;

namespace Whizsheet.Api.Dtos.SavingThrows
{
	public class SavingThrowsDto
	{
		public int Strength { get; set; }
		public bool IsStrengthProficient { get; set; }
		public int Dexterity { get; set; }
		public bool IsDexterityProficient { get; set; }
		public int Constitution { get; set; }
		public bool IsConstitutionProficient { get; set; }
		public int Intelligence { get; set; }
		public bool IsIntelligenceProficient { get; set; }
		public int Wisdom { get; set; }
		public bool IsWisdomProficient { get; set; }
		public int Charisma { get; set; }
		public bool IsCharismaProficient { get; set; }

	}
}
