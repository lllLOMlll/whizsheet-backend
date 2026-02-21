namespace Whizsheet.Api.Domain.Extensions
{
	public class Skill
	{
		public int Id { get; set; }

		public bool isProficientAcrobatics { get; set; }
		public int AcrobaticsModifier => AbilityScores.DexterityModifier;
		public bool isProficientAnimalHandling { get; set; }
		public int AnimalHandlingModifier => AbilityScores.WisdomModifier;
		public bool isProficientArcana {  get; set; }
		public int ArcanaModifier => AbilityScores.IntelligenceModifer;
		public bool isProficientAthletics { get; set; }
		public int AthleticsModifier => AbilityScores.StrengthModifier;
		public bool isProficientDeception { get; set; }
		public int DeceptionModifier => AbilityScores.CharismaModifier;
		public bool isProficientHistory { get; set; }
		public int HistoryModifier => AbilityScores.IntelligenceModifer;
		public bool isProficientInsight { get; set; }
		public int InsightModifier => AbilityScores.WisdomModifier;
		public bool isProficientIntimidation { get; set; }
		public int IntimidationModifier => AbilityScores.CharismaModifier;
		public bool isProficientInvestigation { get; set; }
		public int InvestigationModifier => AbilityScores.IntelligenceModifer;
		public bool isProficientMedecine { get; set; }
		public int MedecineModifier => AbilityScores.WisdomModifier;
		public bool isProficientNature { get; set; }
		public int NatureModifier => AbilityScores.IntelligenceModifer;
		public bool isProficientPerception { get; set; }
		public int PerceptionModifier => AbilityScores.WisdomModifier;
		public bool isProficientPerformance { get; set; }
		public int PerformanceModifier => AbilityScores.CharismaModifier;
		public bool isProficientPersuasion { get; set; }
		public int PersuasionModifier => AbilityScores.WisdomModifier;
		public bool isProficientReligion { get; set; }
		public int ReligionModifier => AbilityScores.IntelligenceModifer;
		public bool isProficientSleighOfHand { get; set; }
		public int SleighOfHandModifier => AbilityScores.DexterityModifier;
		public bool isProficientStealth { get; set; }
		public int StealthModifier => AbilityScores.DexterityModifier;
		public bool isProficientSurvival { get; set; }
		public int SurvivalModifier => AbilityScores.WisdomModifier;

		public int CharacterId { get; set; }
		public Character Character { get; set; } = null!;
		public AbilityScores AbilityScores { get; set; } = null!;

		
	}
}
