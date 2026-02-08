using System.ComponentModel.DataAnnotations;
using Whizsheet.Api.Enum;

namespace Whizsheet.Api.Dtos.CharacterClasses
{
	public class CreateCharacterClassDto
	{
		[Required]
		public CharacterClassType ClassType { get; set; }
		// If class == Other -> CustomClassName
		public string? CustomClassName { get; set; }
		[Range(1, 100)]
		public int Level { get; set; }
	}
}
