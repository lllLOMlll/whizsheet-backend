using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;

namespace Whizsheet.Api.Dtos.Characters
{
	public class UpdateCharacterDto
	{
		[Required]
		public string Name { get; set; } = string.Empty;

	}
}
