using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using Whizsheet.Api.Enum;

namespace Whizsheet.Api.Domain
{
	public class CharacterClass
	{
		public int Id { get; set; }
		[Range(1, 100)]
		public CharacterClassType ClassType { get; set; }
		// If class == Other -> CustomClassName
		public string? CustomClassName { get; set; } ;
		public int Level {  get; set; }
		[Required]
		


		public int CharacterId { get; set; }
		public Character Character { get; set; } = null!;
	


		public string DisplayName()
		{
			return ClassType == CharacterClassType.Other
				? CustomClassName!
				: ClassType.ToString();
		}
	}
}
