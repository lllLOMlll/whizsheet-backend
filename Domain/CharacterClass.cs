using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using Whizsheet.Api.Enum;

namespace Whizsheet.Api.Domain
{
	public class CharacterClass
	{
		public int Id { get; set; }
		public CharacterClassType ClassType { get; set; }
		// If class == Other -> CustomClassName
		public string? CustomClassName { get; set; }
		[Range(1, 100)]
		public string DisplayName =>
			 ClassType == CharacterClassType.Other
				? CustomClassName!
				: ClassType.ToString();
		public int Level {  get; set; }
		
		


		public int CharacterId { get; set; }
		public Character Character { get; set; } = null!;

		
		private CharacterClass() { }

		public CharacterClass(string name)
		{
			ClassType = CharacterClassType.Other;
			CustomClassName = name;
			Level = 1;
		}

		public CharacterClass(int characterId, CharacterClassType type,  int level, string? customName = null)
		{
			CharacterId = characterId;
			ClassType = type;
			CustomClassName = customName;
			Level = level;
		}

	}
}
