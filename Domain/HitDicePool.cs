namespace Whizsheet.Api.Domain
{
	public class HitDicePool
	{
		public int Id { get; set; }

		public int DiceSize { get; set; }
		public int Total {  get; set; }
		public int Remaining {  get; set; }


		public int CharacterId { get; set; }
		public Character Character { get; set; } = null!;


		private HitDicePool() { }

		public HitDicePool(int diceSize, int total)
		{
			DiceSize = diceSize;
			Total = total;
			Remaining = total;
		}
		public void IncreaseTotal(int amount)
		{
			if (amount <= 0) return;

			Total += amount;
			Remaining += amount;
		}

		public void DecreaseTotal(int amount)
		{
			if (amount <= 0) return;

			if (amount >= Total)
			{
				Total = 0;
				Remaining = 0;
				return;
			}

			Total -= amount;

			if (Remaining > Total)
				Remaining = Total;
		}


		public bool Spend()
		{
			if (Remaining <= 0)
				return false;

			Remaining--;
			return true;
		}
		
		public void RestoreAll()
		{
			Remaining = Total;
		}
	}
}
