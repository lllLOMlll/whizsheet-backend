using Whizsheet.Api.Enum;

namespace Whizsheet.Api.Dtos.SavingThrows
{
	public class SavingThrowDto
	{
		public SavingThrowType SavingThrowType { get; set; }
		public bool IsProficient { get; set; }
		public int Modifier {  get; set; }
	}
}
