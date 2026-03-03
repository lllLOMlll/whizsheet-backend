using Whizsheet.Api.Domain.Rules;
using Whizsheet.Api.Dtos.SavingThrows;
using Whizsheet.Api.Enum;

namespace Whizsheet.Api.Domain.Extensions
{
	public static class CharacterClassTypeExtensions
	{
		public static CharacterClassDefinition GetDefinition(this CharacterClassType type)
		{
			return CharacterClassRules.GetDefinition(type);
		}

		public static bool IsProficientIn(
			this CharacterClassType type,
			SavingThrowType savingThrow
			)
		{
			return type
				.GetDefinition()
				.SavingThrows
				.Contains(savingThrow);
		}

		public static int GetHitDie(this CharacterClassType classType)
		{
			return classType switch
			{
				CharacterClassType.Barbarian => 12,

				CharacterClassType.Fighter => 10,
				CharacterClassType.Paladin => 10,
				CharacterClassType.Ranger => 10,

				CharacterClassType.Artificer => 8,
				CharacterClassType.Bard => 8,
				CharacterClassType.BloodHunter => 8,
				CharacterClassType.Cleric => 8,
				CharacterClassType.Druid => 8,
				CharacterClassType.Monk => 8,
				CharacterClassType.Rogue => 8,
				CharacterClassType.Warlock => 8,
				
				CharacterClassType.Sorcerer => 6,
				CharacterClassType.Wizard => 6,

				CharacterClassType.Other => 8,

				_ => throw new ArgumentOutOfRangeException(nameof(classType))
			};
		}
	}
}
