namespace Whizsheet.Api.Domain
{
	public class Skill
	{
		public int Id { get; set; }

		public bool isProficientAcrobatics { get; set; }
		public int AcrobaticsModifier => isProficientAcrobatics ? 
			Character.AbilityScores.DexterityModifier + Character.ProficiencyBonus:
			Character.AbilityScores.DexterityModifier;
		public bool isProficientAnimalHandling { get; set; }
		public int AnimalHandlingModifier => Character.AbilityScores.WisdomModifier;
		public bool isProficientArcana {  get; set; }
		public int ArcanaModifier => Character.AbilityScores.IntelligenceModifier;
		public bool isProficientAthletics { get; set; }
		public int AthleticsModifier => Character.AbilityScores.StrengthModifier;
		public bool isProficientDeception { get; set; }
		public int DeceptionModifier => Character.AbilityScores.CharismaModifier;
		public bool isProficientHistory { get; set; }
		public int HistoryModifier => Character.AbilityScores.IntelligenceModifier;
		public bool isProficientInsight { get; set; }
		public int InsightModifier => Character.AbilityScores.WisdomModifier;
		public bool isProficientIntimidation { get; set; }
		public int IntimidationModifier => Character.AbilityScores.CharismaModifier;
		public bool isProficientInvestigation { get; set; }
		public int InvestigationModifier => Character.AbilityScores.IntelligenceModifier;
		public bool isProficientMedecine { get; set; }
		public int MedecineModifier => Character.AbilityScores.WisdomModifier;
		public bool isProficientNature { get; set; }
		public int NatureModifier => Character.AbilityScores.IntelligenceModifier;
		public bool isProficientPerception { get; set; }
		public int PerceptionModifier => Character.AbilityScores.WisdomModifier;
		public bool isProficientPerformance { get; set; }
		public int PerformanceModifier => Character.AbilityScores.CharismaModifier;
		public bool isProficientPersuasion { get; set; }
		public int PersuasionModifier => Character.AbilityScores.CharismaModifier;
		public bool isProficientReligion { get; set; }
		public int ReligionModifier => Character.AbilityScores.IntelligenceModifier;
		public bool isProficientSleighOfHand { get; set; }
		public int SleighOfHandModifier => Character.AbilityScores.DexterityModifier;
		public bool isProficientStealth { get; set; }
		public int StealthModifier => Character.AbilityScores.DexterityModifier;
		public bool isProficientSurvival { get; set; }
		public int SurvivalModifier => Character.AbilityScores.WisdomModifier;

		public int CharacterId { get; set; }
		public Character Character { get; set; } = null!;

		private Skill () { }

		public Skill(bool isCreationState)
		{			
			this.isProficientAcrobatics = isCreationState;
			this.isProficientAnimalHandling = isCreationState;
			this.isProficientArcana = isCreationState;
			this.isProficientAthletics = isCreationState;
			this.isProficientDeception = isCreationState;
			this.isProficientHistory = isCreationState;
			this.isProficientInsight = isCreationState;
			this.isProficientIntimidation = isCreationState;
			this.isProficientInvestigation = isCreationState;
			this.isProficientMedecine = isCreationState;
			this.isProficientNature = isCreationState;
			this.isProficientPerception = isCreationState;
			this.isProficientPerformance = isCreationState;
			this.isProficientPersuasion = isCreationState;
			this.isProficientReligion = isCreationState;
			this.isProficientSleighOfHand = isCreationState;
			this.isProficientStealth = isCreationState;
			this.isProficientSurvival = isCreationState;	
		}
	}
}
