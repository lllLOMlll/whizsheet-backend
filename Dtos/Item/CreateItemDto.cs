using Whizsheet.Api.Dtos.MagicItem;
using Whizsheet.Api.Enum.Item;

namespace Whizsheet.Api.Dtos.Item
{
	public class CreateItemDto
	{
		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public ItemRarityType ItemRarity { get; set; }

		public int Value { get; set; }

		public double Weight { get; set; }

		public bool IsEquipped { get; set; }

		public bool IsAttuned { get; set; }

		public int Quantity { get; set; }

		public MagicItemDto? MagicItem { get; set; }
	}
}