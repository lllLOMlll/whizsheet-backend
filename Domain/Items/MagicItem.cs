using Whizsheet.Api.Enum.Item;

namespace Whizsheet.Api.Domain.Items
{
	public class MagicItem
	{
		public Guid Id { get; set; }
		public bool RequiresAttunement { get; set; }
		public Guid ItemId { get; set; }
		public Item Item { get; set; } = null!;
		public ICollection<MagicItemEffect> MagicItemEffects { get; set; } = new List<MagicItemEffect>();
	}
}

