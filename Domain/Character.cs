using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace Whizsheet.Api.Domain
{
	public class Character
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int Hp { get; set; }


		// *****************************************
		// COMPOSITION
		//******************************************
		// ABILITY SCORES
		public AbilityScores AbilityScores { get; set; } = null!;
		// CLASSES AND LEVEL
		public List<CharacterClass> Classes { get; set; } = new();
		public const int MaxTotalLevel = 100;
		public int TotalLevel => Classes.Sum(c => c.Level);



		// *****************************************
		// FOREIGN KEY
		// *****************************************
		[ForeignKey(nameof(User))]
		public string UserId { get; set; } = null!;
		public ApplicationUser User { get; set; } = null!;

		


	}
}
