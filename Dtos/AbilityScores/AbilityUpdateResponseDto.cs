using Microsoft.Identity.Client;
using Whizsheet.Api.Dtos;
using Whizsheet.Api.Dtos.Skills;
using Whizsheet.Api.Dtos.SavingThrows;

namespace Whizsheet.Api.Dtos.AbilityScores
{
	public class AbilityUpdateResponseDto
	{
		public UpdateAbilityScoresDto Abilities { get; set; } = null!;
		public List<SkillWithModifierDto> Skills { get; set; } = new();
		public SavingThrowsDto SavingThrows { get; set; } = null!;

	}
}
