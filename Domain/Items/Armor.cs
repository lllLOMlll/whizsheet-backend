using Whizsheet.Api.Enum.Armor;


namespace Whizsheet.Api.Domain.Items
{
	public class Armor : Item
	{
		public ArmorCategoryType ArmorCategory { get; set; }
		public ArmorTypeType ArmorType { get; set; }
		public int ArmorClass {  get; set; }
		public bool HasStealthDisavantage { get; set; }
		public int? StrengthRequirement {  get; set; }

	}
}
