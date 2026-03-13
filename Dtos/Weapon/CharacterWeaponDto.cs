namespace Whizsheet.Api.Dtos.Weapon
{
	public class CharacterWeaponDto
	{
		public Guid CharacterItemId { get; set; }

		public WeaponDto Weapon { get; set; } = null!;

		public int Quantity { get; set; }

		public bool IsEquipped { get; set; }

		public bool IsAttuned { get; set; }

		public int? ChargesRemaining { get; set; }
	}
}
