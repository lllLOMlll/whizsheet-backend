using Whizsheet.Api.Enum.Item;

namespace Whizsheet.Api.Domain.Items
{
	public class Potion : Item
	{
		public PotionType PotionType { get; set; }
		public string EffectDescription { get; set; } = string.Empty;
	}
}
