using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whizsheet.Api.Migrations
{
    /// <inheritdoc />
    public partial class SkillsNameMinorFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isProficientSurvival",
                table: "Skills",
                newName: "isSurvivalProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientStealth",
                table: "Skills",
                newName: "isStealthProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientSleighOfHand",
                table: "Skills",
                newName: "isSleighOfHandProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientReligion",
                table: "Skills",
                newName: "isReligionProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientPersuasion",
                table: "Skills",
                newName: "isPersuasionProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientPerformance",
                table: "Skills",
                newName: "isPerformanceProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientPerception",
                table: "Skills",
                newName: "isPerceptionProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientNature",
                table: "Skills",
                newName: "isNatureProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientMedecine",
                table: "Skills",
                newName: "isMedecineProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientInvestigation",
                table: "Skills",
                newName: "isInvestigationProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientIntimidation",
                table: "Skills",
                newName: "isIntimidationProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientInsight",
                table: "Skills",
                newName: "isInsightProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientHistory",
                table: "Skills",
                newName: "isHistoryProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientDeception",
                table: "Skills",
                newName: "isDeceptionProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientAthletics",
                table: "Skills",
                newName: "isAthleticsProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientArcana",
                table: "Skills",
                newName: "isArcanaProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientAnimalHandling",
                table: "Skills",
                newName: "isAnimalHandlingProficient");

            migrationBuilder.RenameColumn(
                name: "isProficientAcrobatics",
                table: "Skills",
                newName: "isAcrobaticsProficient");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isSurvivalProficient",
                table: "Skills",
                newName: "isProficientSurvival");

            migrationBuilder.RenameColumn(
                name: "isStealthProficient",
                table: "Skills",
                newName: "isProficientStealth");

            migrationBuilder.RenameColumn(
                name: "isSleighOfHandProficient",
                table: "Skills",
                newName: "isProficientSleighOfHand");

            migrationBuilder.RenameColumn(
                name: "isReligionProficient",
                table: "Skills",
                newName: "isProficientReligion");

            migrationBuilder.RenameColumn(
                name: "isPersuasionProficient",
                table: "Skills",
                newName: "isProficientPersuasion");

            migrationBuilder.RenameColumn(
                name: "isPerformanceProficient",
                table: "Skills",
                newName: "isProficientPerformance");

            migrationBuilder.RenameColumn(
                name: "isPerceptionProficient",
                table: "Skills",
                newName: "isProficientPerception");

            migrationBuilder.RenameColumn(
                name: "isNatureProficient",
                table: "Skills",
                newName: "isProficientNature");

            migrationBuilder.RenameColumn(
                name: "isMedecineProficient",
                table: "Skills",
                newName: "isProficientMedecine");

            migrationBuilder.RenameColumn(
                name: "isInvestigationProficient",
                table: "Skills",
                newName: "isProficientInvestigation");

            migrationBuilder.RenameColumn(
                name: "isIntimidationProficient",
                table: "Skills",
                newName: "isProficientIntimidation");

            migrationBuilder.RenameColumn(
                name: "isInsightProficient",
                table: "Skills",
                newName: "isProficientInsight");

            migrationBuilder.RenameColumn(
                name: "isHistoryProficient",
                table: "Skills",
                newName: "isProficientHistory");

            migrationBuilder.RenameColumn(
                name: "isDeceptionProficient",
                table: "Skills",
                newName: "isProficientDeception");

            migrationBuilder.RenameColumn(
                name: "isAthleticsProficient",
                table: "Skills",
                newName: "isProficientAthletics");

            migrationBuilder.RenameColumn(
                name: "isArcanaProficient",
                table: "Skills",
                newName: "isProficientArcana");

            migrationBuilder.RenameColumn(
                name: "isAnimalHandlingProficient",
                table: "Skills",
                newName: "isProficientAnimalHandling");

            migrationBuilder.RenameColumn(
                name: "isAcrobaticsProficient",
                table: "Skills",
                newName: "isProficientAcrobatics");
        }
    }
}
