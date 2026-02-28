using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;
using Whizsheet.Api.Domain.Extensions;
using Whizsheet.Api.Dtos.Skills;
using Whizsheet.Api.Enum;

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
		public List<Skill> Skills { get; set; } = new();

		// SAVING THROWS
		public List<SavingThrow> SavingThrows { get; set; } = new();






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

			CreateStartingClass("Starting Class");
			CreateStartingHitPoints(10);
			CreateStartingAbilityScores(10, 10, 10, 10, 10, 10);
			CreateStartingSavingThrows();
			CreateStartingSkills();
			
		}

		// =========================
		// FACTORY METHODS
		// =========================

		//************* HIT POINTS *************
		public void CreateStartingClass(string startingClass)
		{
			if (Classes.Any())
				throw new InvalidOperationException("Classes already created");

			var characterStartingClass = new CharacterClass("Starting Class");

			Classes.Add(characterStartingClass);

		}

		
		//************* HIT POINTS *************
		public void CreateStartingHitPoints(int totalHp)
		{
			if (HitPoints != null)
				throw new InvalidOperationException("HitPoints already exist.");

			if (totalHp <= 0 || totalHp > MaxHp)
				throw new ArgumentOutOfRangeException(nameof(totalHp));

			HitPoints = new HitPoints(totalHp);
		}


		////************* ABILITY SCORES //*************
		public void CreateStartingAbilityScores(
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

		public SkillsDtoWithModifiers ToSkillsDto()
		{
			return new SkillsDtoWithModifiers
			{
				Skills = this.Skills.Select(s => new SkillWithModifierDto
				{
					Type = s.Type,
					IsProficient = s.IsProficient,
					Modifier = this.GetSkillModifier(s.Type)
				}).ToList()
			};
		}

		//************* SAVING THROWS *************

		public void CreateStartingSavingThrows()
		{
			if (SavingThrows.Any())
				throw new InvalidOperationException("Saving throws aleready created");

			foreach (SavingThrowType type in System.Enum.GetValues<SavingThrowType>())
			{
				SavingThrows.Add(new SavingThrow(type));
			}

			var mainCharacterClass = GetCharacterMainClass();
		
			foreach (SavingThrow s in SavingThrows)
			{
				if (CharacterClassTypeExtensions.IsProficientIn(
					mainCharacterClass,
					s.Type))
				{
					s.SetProficiency(true);
				}
			}	
		}

		public CharacterClassType GetCharacterMainClass()
		{
			if (!Classes.Any())
				throw new InvalidOperationException("Character has no classes.");

			return Classes
				.OrderByDescending(c => c.Level)
				.First()
				.ClassType;
			
		}

		public int getStrengthSavingThrowsBonus()
		{
			var mainCharacterClass = GetCharacterMainClass();

			if (mainCharacterClass == CharacterClassType.Barbarian ||
				mainCharacterClass == CharacterClassType.Fighter ||
				mainCharacterClass == CharacterClassType.Monk ||
				mainCharacterClass == CharacterClassType.Ranger)
			{
				return ProficiencyBonus;
			}

			return 0;
		}

		public int getDexteritySavingThrowsBonus()
		{
			var mainCharacterClass = GetCharacterMainClass();

			if (mainCharacterClass == CharacterClassType.Bard ||
				mainCharacterClass == CharacterClassType.BloodHunter ||
				mainCharacterClass == CharacterClassType.Monk ||
				mainCharacterClass == CharacterClassType.Ranger ||
				mainCharacterClass == CharacterClassType.Rogue)
			{
				return ProficiencyBonus;
			}

			return 0;
		}

		public int getConstitutionSavingThrowsBonus()
		{
			var mainCharacterClass = GetCharacterMainClass();

			if (mainCharacterClass == CharacterClassType.Artificer ||
				mainCharacterClass == CharacterClassType.Barbarian ||
				mainCharacterClass == CharacterClassType.Fighter ||				
				mainCharacterClass == CharacterClassType.Sorcerer)
			{
				return ProficiencyBonus;
			}

			return 0;
		}

		public int getIntelligenceSavingThrowsBonus()
		{
			var mainCharacterClass = GetCharacterMainClass();

			if (mainCharacterClass == CharacterClassType.Artificer ||
				mainCharacterClass == CharacterClassType.BloodHunter ||
				mainCharacterClass == CharacterClassType.Druid ||
				mainCharacterClass == CharacterClassType.Rogue||
				mainCharacterClass == CharacterClassType.Wizard
				)
			{
				return ProficiencyBonus;
			}

			return 0;
		}

		public int getWisdomSavingThrowsBonus()
		{
			var mainCharacterClass = GetCharacterMainClass();

			if (mainCharacterClass == CharacterClassType.Cleric ||
				mainCharacterClass == CharacterClassType.Druid||
				mainCharacterClass == CharacterClassType.Paladin ||
				mainCharacterClass == CharacterClassType.Warlock||
				mainCharacterClass == CharacterClassType.Wizard 
			)
			{
				return ProficiencyBonus;
			}

			return 0;
		}

		public int getCharismaSavingThrowsBonus()
		{
			var mainCharacterClass = GetCharacterMainClass();

			if (mainCharacterClass == CharacterClassType.Bard ||
				mainCharacterClass == CharacterClassType.Cleric||
				mainCharacterClass == CharacterClassType.Paladin ||
				mainCharacterClass == CharacterClassType.Sorcerer||
				mainCharacterClass == CharacterClassType.Warlock
			)
			{
				return ProficiencyBonus;
			}

			return 0;
		}


		//************* SKILLS *************
		public int GetSkillModifier(SkillType type)
		{
			if (AbilityScores == null)
				throw new InvalidOperationException("AbilityScores not initialized.");

			int abilityModifier = type switch
			{
				SkillType.Acrobatics => AbilityScores.DexterityModifier,
				SkillType.AnimalHandling => AbilityScores.WisdomModifier,
				SkillType.Arcana => AbilityScores.IntelligenceModifier,
				SkillType.Athletics => AbilityScores.StrengthModifier,
				SkillType.Deception => AbilityScores.CharismaModifier,
				SkillType.History => AbilityScores.IntelligenceModifier,
				SkillType.Insight => AbilityScores.WisdomModifier,
				SkillType.Intimidation => AbilityScores.CharismaModifier,
				SkillType.Investigation => AbilityScores.IntelligenceModifier,
				SkillType.Medicine => AbilityScores.WisdomModifier,
				SkillType.Nature => AbilityScores.IntelligenceModifier,
				SkillType.Perception => AbilityScores.WisdomModifier,
				SkillType.Performance => AbilityScores.CharismaModifier,
				SkillType.Persuasion => AbilityScores.CharismaModifier,
				SkillType.Religion => AbilityScores.IntelligenceModifier,
				SkillType.SleightOfHand => AbilityScores.DexterityModifier,
				SkillType.Stealth => AbilityScores.DexterityModifier,
				SkillType.Survival => AbilityScores.WisdomModifier,
				_ => throw new InvalidOperationException($"Unhandled skill type: {type}")
			};

			var skill = Skills.FirstOrDefault(s => s.Type == type);

			if (skill == null)
				throw new InvalidOperationException("Skill not found.");

			return skill.IsProficient
				? abilityModifier + ProficiencyBonus
				: abilityModifier;
		}


		public void CreateStartingSkills()
		{
			if (Skills.Any())
				throw new InvalidOperationException("Skills already created.");

			foreach (SkillType type in System.Enum.GetValues<SkillType>())

			{
				Skills.Add(new Skill(type));
			}	
		}



		//************* HIT DICES************************

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

