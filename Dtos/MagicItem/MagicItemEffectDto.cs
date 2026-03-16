using Whizsheet.Api.Enum;
using Whizsheet.Api.Enum.Item;

namespace Whizsheet.Api.Dtos.MagicItem
{
	public class MagicItemEffectDto
	{
		public Guid Id { get; set; }

		public ItemEffectType? EffectType { get; set; }

		public AbilityScoreType? AbilityScore { get; set; }

		public SavingThrowType? SavingThrow { get; set; }

		public SkillType? Skill { get; set; }

		public int Modifier { get; set; }
	}
}