using Whizsheet.Api.Dtos;
using Whizsheet.Api.Dtos.Skills;

namespace Whizsheet.Api.Dtos.AbilityScores
{
	public class AbilityUpdateResponseDto
	{
		public UpdateAbilityScoresDto Abilities { get; set; } = null!;
		public List<SkillWithModifierDto> Skills { get; set; } = new();

	}
}
