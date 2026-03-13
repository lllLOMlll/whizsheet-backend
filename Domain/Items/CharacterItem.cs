namespace Whizsheet.Api.Domain.Items
{
	public class CharacterItem
	{
		public Guid Id { get; set; }

		public int CharacterId { get; set; }

		public Character Character { get; set; } = null!;

		public Guid ItemId { get; set; }

		public Item Item { get; set; } = null!;

		public int Quantity { get; set; } = 1;

		public bool IsEquipped { get; set; }

		public bool IsAttuned { get; set; }

		public int? ChargesRemaining { get; set; }
	}
}
