using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;
using Whizsheet.Api.Domain.Extensions;

namespace Whizsheet.Api.Domain
{
	public class Character
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;

		public int ProficiencyBonus
		{
			get
			{
				if (TotalLevel <= 0)
					return 2;

				return 2 + ((TotalLevel - 1) / 4);
			}
		}


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

		// SKILLS
		public Skill Skills { get; set; } = null!;






		// *****************************************
		// FOREIGN KEY
		// *****************************************
		[ForeignKey(nameof(User))]
		public string UserId { get; set; } = null!;
		public ApplicationUser User { get; set; } = null!;

		private Character() { }

		public Character(string name, string userId)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Name is required.", nameof(name));

			if (string.IsNullOrWhiteSpace(userId))
				throw new ArgumentException("UserId is required.", nameof(userId));

			Name = name;
			UserId = userId;
		}

		// =========================
		// FACTORY METHODS
		// =========================
		public void CreateHitPoints(int totalHp)
		{
			if (HitPoints != null)
				throw new InvalidOperationException("HitPoints already exist.");

			if (totalHp <= 0 || totalHp > MaxHp) 
				throw new ArgumentOutOfRangeException(nameof(totalHp));

			HitPoints = new HitPoints(totalHp);
		}

		public void CreateAbilityScores(
		 int strength,
		 int dexterity,
		 int constitution,
		 int intelligence,
		 int wisdom,
		 int charisma)
		{
			if (AbilityScores != null)
				throw new InvalidOperationException("Ability scores already exist.");

			AbilityScores = new AbilityScores(
				strength,
				dexterity,
				constitution,
				intelligence,
				wisdom,
				charisma);
		}

		public void CreateSkills()
		{
			if (Skills != null)
				throw new InvalidOperationException("Skills already exist");
			Skills = new Skill(false);
		}

			
	

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

