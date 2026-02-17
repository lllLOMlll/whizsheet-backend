using Whizsheet.Api.Enum;

namespace Whizsheet.Api.Domain
{
	public class HitPoints
	{
		public int Id { get; set; }
		public int TotalHitPoints { get; set; }
		public int CurrentHitPoints { get; set; }
		public int TemporaryHitPoints { get; set; }
		public int CharacterId { get; set; }
		public Character Character { get; set; } = null!;

		
		private HitPoints() { }
		public HitPoints(int totalHp)
		{
			TotalHitPoints = totalHp;
			CurrentHitPoints = totalHp;
		}

		public void TakeDamage(int amount)
		{
			if (amount <= 0) return;

			if (TemporaryHitPoints > 0)
			{
				int absorbed = Math.Min(amount, TemporaryHitPoints);
				TemporaryHitPoints -= absorbed;
				amount -= absorbed;
			}

			if (amount > 0)
			{
				CurrentHitPoints = Math.Max(0, CurrentHitPoints - amount);
			}
		}


		public void Heal(int amount)
		{
			if (amount <= 0) return;

			CurrentHitPoints = Math.Min(TotalHitPoints, CurrentHitPoints + amount);
		}

		public void AddTemporaryHitPoints(int amount)
		{
			if (amount < 0) return;

			TemporaryHitPoints = Math.Max(TemporaryHitPoints, amount);
		}

		public void RestoreFull()
		{
			CurrentHitPoints = TotalHitPoints;
			TemporaryHitPoints = 0;
		}
	}
}
