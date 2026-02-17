using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;
using Whizsheet.Api.Domain.Extensions;

namespace Whizsheet.Api.Domain
{
	public class Character
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;


		// *****************************************
		// COMPOSITION
		//******************************************
		// ABILITY SCORES
		public AbilityScores AbilityScores { get; set; } = null!;
		// CLASSES AND LEVEL
		public List<CharacterClass> Classes { get; set; } = new();
		public const int MaxTotalLevel = 100;
		public int TotalLevel => Classes.Sum(c => c.Level);
		// HIT POINTS
		public HitPoints HitPoints { get; set; } = null!;
		public const int MaxHp = 999;
		// HIT DICE POOL
		public IReadOnlyCollection<HitDicePool> HitDicePools => _hitDicePools;
		private readonly List<HitDicePool> _hitDicePools = new();






		// *****************************************
		// FOREIGN KEY
		// *****************************************
		[ForeignKey(nameof(User))]
		public string UserId { get; set; } = null!;
		public ApplicationUser User { get; set; } = null!;




		public void SyncHitDicePools()
		{
			var breakdown = Classes
				.GroupBy(c => c.ClassType.GetHitDie())
				.ToDictionary(
					g => g.Key,
					g => g.Sum(c => c.Level)
				);

			foreach (var entry in breakdown)
			{
				var existing = _hitDicePools
					.FirstOrDefault(p => p.DiceSize == entry.Key);

				if (existing == null)
				{
					_hitDicePools.Add(new HitDicePool(entry.Key, entry.Value));
				}

				else
				{
					int diff = entry.Value - existing.Total;

					existing.Total = entry.Value;

					if (diff > 0)
						existing.Remaining += diff;

					if (existing.Remaining > existing.Total)
						existing.Remaining = existing.Total;
				}
			}

			
			_hitDicePools.RemoveAll(p => !breakdown.ContainsKey(p.DiceSize));
		}

		public bool SpendHitDie(int diceSize)
		{
			var pool = _hitDicePools
				.FirstOrDefault(p => p.DiceSize == diceSize);

			if (pool == null)
				return false;

			if (pool.Remaining <= 0)
				return false;

			pool.Remaining--;

			return true;
		}

		public void RecoverAllHitDice()
		{
			foreach (var pool in _hitDicePools)
			{
				pool.Remaining = pool.Total;
			}
		}


	}
}

