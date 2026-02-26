using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whizsheet.Api.Migrations
{
    /// <inheritdoc />
    public partial class SkillsRefactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Skills_CharacterId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isAcrobaticsProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isAnimalHandlingProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isArcanaProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isAthleticsProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isDeceptionProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isHistoryProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isInsightProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isIntimidationProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isInvestigationProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isMedecineProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isNatureProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isPerceptionProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isPerformanceProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isPersuasionProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isReligionProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isSleighOfHandProficient",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "isStealthProficient",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "isSurvivalProficient",
                table: "Skills",
                newName: "IsProficient");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Skills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CharacterId",
                table: "Skills",
                column: "CharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Skills_CharacterId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "IsProficient",
                table: "Skills",
                newName: "isSurvivalProficient");

            migrationBuilder.AddColumn<bool>(
                name: "isAcrobaticsProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isAnimalHandlingProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isArcanaProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isAthleticsProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeceptionProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isHistoryProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isInsightProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isIntimidationProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isInvestigationProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isMedecineProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isNatureProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isPerceptionProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isPerformanceProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isPersuasionProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isReligionProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isSleighOfHandProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isStealthProficient",
                table: "Skills",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CharacterId",
                table: "Skills",
                column: "CharacterId",
                unique: true);
        }
    }
}
