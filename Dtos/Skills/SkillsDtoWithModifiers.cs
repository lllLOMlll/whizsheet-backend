using Whizsheet.Api.Domain;

namespace Whizsheet.Api.Dtos.Skills
{
	public class SkillsDtoWithModifiers
	{
		public List<SkillWithModifierDto> Skills { get; set; } = new();
	}
}
