using System.ComponentModel.DataAnnotations;
using Whizsheet.Api.Enum;

namespace Whizsheet.Api.Domain
{
	public class AbilityScores
	{
		public int Id { get; set; }
		
		public int Strength { get; set; }
		public int StrengthModifier => (int)Math.Floor((Strength - 10) / 2.0) + Character.GetMagicItemBonusAbilityScore(AbilityScoreType.Strength);
		public int StrengthSavingThrowsModifier =>
			StrengthModifier + Character.getSavingThrowBonus(SavingThrowType.Strength);
		public int Dexterity { get; set; }
		public int DexterityModifier => (int)Math.Floor((Dexterity - 10) / 2.0);
		public int DexteritySavingThrowsModifier =>
			DexterityModifier + Character.getSavingThrowBonus(SavingThrowType.Dexterity);
		public int Constitution { get; set; }
		public int ConstitutionModifier => (int)Math.Floor((Constitution - 10) / 2.0);
		public int ConstitutionSavingThrowsModifier =>
			ConstitutionModifier + Character.getSavingThrowBonus(SavingThrowType.Constitution);
		public int Intelligence{ get; set; }
		public int IntelligenceModifier => (int)Math.Floor((Intelligence - 10) / 2.0);
		public int IntelligenceSavingThrowsModifier =>
			IntelligenceModifier + Character.getSavingThrowBonus(SavingThrowType.Intelligence);
		public int Wisdom { get; set; }
		public int WisdomModifier => (int)Math.Floor((Wisdom - 10) / 2.0);
		public int WisdomSavingThrowsModifier =>
			WisdomModifier + Character.getSavingThrowBonus(SavingThrowType.Wisdom);
		public int Charisma { get; set; }
		public int CharismaModifier => (int)Math.Floor((Charisma - 10) / 2.0);
		public int CharismaSavingThrowsModifier =>
			DexterityModifier + Character.getSavingThrowBonus(SavingThrowType.Charisma);
		public int CharacterId { get; set; }
		public Character Character { get; set; } = null!;


		private AbilityScores() { }
		public AbilityScores(
				int strength,
				int dexterity,
				int constitution,
				int intelligence,
				int wisdom,
				int charisma)
		{
			Strength = strength;
			Dexterity = dexterity;
			Constitution = constitution;
			Intelligence = intelligence;
			Wisdom = wisdom;
			Charisma = charisma;
		}


	}
}
