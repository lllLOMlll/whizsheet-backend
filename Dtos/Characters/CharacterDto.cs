using Whizsheet.Api.Dtos.CharacterClasses;

namespace Whizsheet.Api.Dtos.Characters
{
	public class CharacterDto
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int? TotalHitPoints { get; set; } 
		public List<CharacterClassDto>? CharacterClass { get; set; }
	}
}
