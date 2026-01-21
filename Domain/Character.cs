using System.ComponentModel.DataAnnotations.Schema;

namespace Whizsheet.Api.Domain
{
	public class Character
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Class { get; set; } = string.Empty;
		public int Hp {  get; set; }

		[ForeignKey(nameof(User))]
		public string UserId { get; set; } = null!;
		public ApplicationUser User { get; set; } = null!;
	}
}
