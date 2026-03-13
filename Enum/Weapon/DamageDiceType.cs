using System.ComponentModel.DataAnnotations;

namespace Whizsheet.Api.Enum.Weapon
{
	public enum DamageDiceType
	{
		[Display(Name = "1d4")]
		D4_1 = 1,
		[Display(Name = "1d6")]
		D6_1 = 2,
		[Display(Name = "1d8")]
		D8_1 = 3,
		[Display(Name = "1d10")]
		D10_1 = 4,
		[Display(Name = "1d12")]
		D12_1 = 5,
		[Display(Name = "2d4")]
		D4_2 = 6,
		[Display(Name = "2d6")]
		D6_2 = 7,
		[Display(Name = "2d8")]
		D8_2 = 8,
		[Display(Name = "2d10")]
		D10_2 = 9,
		[Display(Name = "2d12")]
		D12_2 = 10,
	}
}
