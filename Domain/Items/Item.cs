using Whizsheet.Api.Enum.Item;

namespace Whizsheet.Api.Domain.Items
{
	public abstract class Item
	{
		public Guid Id { get; set; }

		public int CharacterId { get; set; }

		public Character Character { get; set; } = null!;

		public string Name { get; set; } = string.Empty;
		public string Description {  get; set; } = string.Empty;
		public ItemRarityType ItemRarity {  get; set; }
		public int Value { get; set; }
		public double Weight { get; set; }

		public bool IsEquipped { get; set; }

		public bool IsAttuned { get; set; }

		public int Quantity { get; set; }
	


		public MagicItem? MagicItem { get; set; }

	}
}
