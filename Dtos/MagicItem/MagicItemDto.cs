namespace Whizsheet.Api.Dtos.MagicItem
{
	public class MagicItemDto
	{
		public bool RequiresAttunement { get; set; }

		public List<MagicItemEffectDto> Effects { get; set; } = new();
	}
}
