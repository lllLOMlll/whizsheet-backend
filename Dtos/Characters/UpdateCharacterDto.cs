using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;

namespace Whizsheet.Api.Dtos.Characters
{
	public class UpdateCharacterDto
	{
		[Required]
		public string Name { get; set; } = string.Empty;
		[Required]
		[Range(1, 999)]
		public int Hp { get; set; }
	}
}
