using System.ComponentModel.DataAnnotations;

namespace Whizsheet.Api.Dtos.Characters
{
	public class CreateCharacterDto
	{
		[Required]
		public string Name { get; set; } = string.Empty;

	}
}
