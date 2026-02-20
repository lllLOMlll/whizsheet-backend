using System.ComponentModel.DataAnnotations; 

namespace Whizsheet.Api.Domain
{
	public class AbilityScores
	{
		public int Id { get; set; }
		
		public int Strength { get; set; }
		public int StrengthModifier => (int)Math.Floor((Strength - 10) / 2.0);
		public int Dexterity { get; set; }
		public int DexterityModifier => (int)Math.Floor((Dexterity - 10) / 2.0);
		public int Constitution { get; set; }
		public int ConstitutionModifier => (int)Math.Floor((Constitution - 10) / 2.0);
		public int Intelligence{ get; set; }
		public int IntelligenceModifer => (int)Math.Floor((Intelligence - 10) / 2.0);
		public int Wisdom { get; set; }
		public int WisdomModifier => (int)Math.Floor((Wisdom - 10) / 2.0);
		public int Charisma { get; set; }
		public int CharismaModifier => (int)Math.Floor((Charisma - 10) / 2.0);
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
