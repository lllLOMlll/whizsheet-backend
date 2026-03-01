using System.Diagnostics;
using Whizsheet.Api.Enum;

namespace Whizsheet.Api.Domain
{
	public class SavingThrow
	{
		public int Id { get; set; }
		
		public SavingThrowType SavingThrowType { get; private set; }
		public bool IsProficient { get; private set; }
		public int CharacterId { get; private set; }
		public Character Character { get; private set; } = null!;

		private SavingThrow() { }

		public SavingThrow(SavingThrowType type)
		{
			SavingThrowType = type;
			IsProficient = false;
		}

		public void SetProficiency(bool value)
		{
			IsProficient = value;
		}
	}
}
