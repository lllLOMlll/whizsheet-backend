using Whizsheet.Api.Enum.Weapon;

namespace Whizsheet.Api.Domain.Items
{
	public class Weapon : Item
	{

		public AttackType? AttackType { get; set; }
		public BonusAttackRollType? BonusAttackRollType {  get; set; }
		public DamageDiceType? DamageDiceType { get; set; }
		public DamageType? DamageType { get; set; }
		public RangeType? RangeType { get; set; }
		public bool IsLight { get; set; }
		public bool IsFinesse { get; set; }
		public bool IsThrown { get; set; }
		public bool IsVersatile { get; set; }
		public bool IsAmmunition { get; set; }
		public bool IsHeavy { get; set; }
		public bool IsReach { get; set; }
		public bool IsTwoHanded { get; set; }
		public bool IsLoading { get; set; }
		public bool IsSpecial { get; set; }
	}
}
