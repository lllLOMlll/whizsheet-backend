using Whizsheet.Api.Enum;

namespace Whizsheet.Api.Dtos.Skills
{
	public class SkillWithModifierDto
	{
		public SkillType Type { get; set; }
		public bool IsProficient { get; set; }
		public int Modifier { get; set; }
	}
}
