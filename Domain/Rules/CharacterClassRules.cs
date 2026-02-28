using Whizsheet.Api.Domain.Extensions;
using Whizsheet.Api.Enum;

namespace Whizsheet.Api.Domain.Rules
{
	public class CharacterClassRules
	{

		private static readonly Dictionary<CharacterClassType, CharacterClassDefinition> Definitions =
			new Dictionary<CharacterClassType, CharacterClassDefinition>
			{
				{
					CharacterClassType.Artificer,
					new CharacterClassDefinition
					{
						HitDie = 8,
						SavingThrows = new[]
						{
							SavingThrowType.Constitution,
							SavingThrowType.Intelligence
						}
					}
				},
				{
					CharacterClassType.Barbarian,
					new CharacterClassDefinition
					{
						HitDie = 12,
						SavingThrows = new[]
						{
							SavingThrowType.Strength,
							SavingThrowType.Constitution
						}
					}
				},
				{
					CharacterClassType.Bard,
					new CharacterClassDefinition
					{
						HitDie = 8,
						SavingThrows = new[]
						{
							SavingThrowType.Dexterity,
							SavingThrowType.Charisma
						}
					}
				},
				{
					CharacterClassType.BloodHunter,
					new CharacterClassDefinition
					{
						HitDie = 10,
						SavingThrows = new[]
						{
							SavingThrowType.Dexterity,
							SavingThrowType.Intelligence
						}
					}
				},
				{
					CharacterClassType.Cleric,
					new CharacterClassDefinition
					{
						HitDie = 8,
						SavingThrows = new[]
						{
							SavingThrowType.Wisdom,
							SavingThrowType.Charisma
						}
					}
				},
				{
					CharacterClassType.Druid,
					new CharacterClassDefinition
					{
						HitDie = 8,
						SavingThrows = new[]
						{
							SavingThrowType.Intelligence,
							SavingThrowType.Wisdom
						}
					}
				},
				{
					CharacterClassType.Fighter,
					new CharacterClassDefinition
					{
						HitDie = 10,
						SavingThrows = new[]
						{
							SavingThrowType.Strength,
							SavingThrowType.Constitution
						}
					}
				},
				{
					CharacterClassType.Monk,
					new CharacterClassDefinition
					{
						HitDie = 8,
						SavingThrows = new[]
						{
							SavingThrowType.Strength,
							SavingThrowType.Dexterity
						}
					}
				},
				{
					CharacterClassType.Paladin,
					new CharacterClassDefinition
					{
						HitDie = 10,
						SavingThrows = new[]
						{
							SavingThrowType.Wisdom,
							SavingThrowType.Charisma
						}
					}
				},
				{
					CharacterClassType.Ranger,
					new CharacterClassDefinition
					{
						HitDie = 10,
						SavingThrows = new[]
						{
							SavingThrowType.Strength,
							SavingThrowType.Dexterity
						}
					}
				},
				{
					CharacterClassType.Rogue,
					new CharacterClassDefinition
					{
						HitDie = 8,
						SavingThrows = new[]
						{
							SavingThrowType.Dexterity,
							SavingThrowType.Intelligence
						}
					}
				},
				{
					CharacterClassType.Sorcerer,
					new CharacterClassDefinition
					{
						HitDie = 6,
						SavingThrows = new[]
						{
							SavingThrowType.Constitution,
							SavingThrowType.Charisma
						}
					}
				},
				{
					CharacterClassType.Warlock,
					new CharacterClassDefinition
					{
						HitDie = 8,
						SavingThrows = new[]
						{
							SavingThrowType.Wisdom,
							SavingThrowType.Charisma
						}
					}
				},
				{
					CharacterClassType.Wizard,
					new CharacterClassDefinition
					{
						HitDie = 6,
						SavingThrows = new[]
						{
							SavingThrowType.Intelligence,
							SavingThrowType.Wisdom
						}
					}
				},
				{
					CharacterClassType.Other,
					new CharacterClassDefinition
					{
						HitDie = 8,
						SavingThrows = Array.Empty<SavingThrowType>()
					}
				}
			};


		public static CharacterClassDefinition GetDefinition(CharacterClassType type)
		{
			if (!Definitions.TryGetValue(type, out var definition))
				throw new ArgumentOutOfRangeException(nameof(type));

			return definition;
		}
	}

}

