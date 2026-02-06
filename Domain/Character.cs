using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace Whizsheet.Api.Domain
{
	public class Character
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int Hp { get; set; }


		// COMPOSITION -> Classes linked to Character
		public AbilityScores AbilityScores { get; set; } = null!;

		public List<CharacterClass> Classes { get; set; } = new();

		// FOREIGN KEY

		[ForeignKey(nameof(User))]
		public string UserId { get; set; } = null!;
		public ApplicationUser User { get; set; } = null!;


		// METHOD
		public int GetTotalLevel => Classes.Sum(c => c.Level);



	}
}
