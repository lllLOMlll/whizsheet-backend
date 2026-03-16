namespace Whizsheet.Api.Dtos.MagicItem
{
	public class MagicItemDto
	{
		public Guid Id { get; set; }

		public bool RequiresAttunement { get; set; }

		public string MagicEffectDescription { get; set; } = string.Empty;

		public string MagicEffectMechanics { get; set; } = string.Empty;

		public int? TotalCharges { get; set; }

		public int? ChargesRemaining { get; set; }

		public string MagicRechargeRate { get; set; } = string.Empty;

		public List<MagicItemEffectDto> Effects { get; set; } = new();
	}
}