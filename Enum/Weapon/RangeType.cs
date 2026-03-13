using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Whizsheet.Api.Enum.Weapon
{
	public enum RangeType
	{
		[Display(Name = "Not a range weapon")]
		R0 = 0,
		[Display(Name = "5/15")]
		R1 = 1,
		[Display(Name = "20/60")]
		R2 = 2,
		[Display(Name = "30/120")]
		R3 = 3,
		[Display(Name = "80/320")]
		R4 = 4,
		[Display(Name = "100/400")]
		R5 = 5,
		[Display(Name = "150/600")]
		R6 = 6,	
	}
}
