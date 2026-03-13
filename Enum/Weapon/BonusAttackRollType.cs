using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Whizsheet.Api.Enum.Weapon
{
	public enum BonusAttackRollType
	{
		[Display(Name = "+0")]
		None = 0,

        [Display(Name = "+1")]
		B1 = 1,

        [Display(Name = "+2")]
		B2 = 2,

        [Display(Name = "+3")]
		B3 = 3,

        [Display(Name = "+4")]
		B4 = 4,

        [Display(Name = "+5")]
		B5 = 5,
	}
}
