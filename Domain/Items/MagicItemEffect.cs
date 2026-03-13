using Whizsheet.Api.Enum;
using Whizsheet.Api.Enum.Item;

namespace Whizsheet.Api.Domain.Items
{
	public class MagicItemEffect
	{
		public Guid Id { get; set; }

		public ItemEffectType? EffectType { get; set; }

		public AbilityScoreType? AbilityScore { get; set; }

		public SavingThrowType? SavingThrow { get; set; }

		public SkillType? Skill { get; set; }

		public int Modifier { get; set; }

		public string Description { get; set; } = string.Empty;

		public Guid MagicItemId { get; set; }

		public MagicItem MagicItem { get; set; } = null!;
	}
}
