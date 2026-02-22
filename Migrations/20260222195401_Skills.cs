using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whizsheet.Api.Migrations
{
    /// <inheritdoc />
    public partial class Skills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isProficientAcrobatics = table.Column<bool>(type: "bit", nullable: false),
                    isProficientAnimalHandling = table.Column<bool>(type: "bit", nullable: false),
                    isProficientArcana = table.Column<bool>(type: "bit", nullable: false),
                    isProficientAthletics = table.Column<bool>(type: "bit", nullable: false),
                    isProficientDeception = table.Column<bool>(type: "bit", nullable: false),
                    isProficientHistory = table.Column<bool>(type: "bit", nullable: false),
                    isProficientInsight = table.Column<bool>(type: "bit", nullable: false),
                    isProficientIntimidation = table.Column<bool>(type: "bit", nullable: false),
                    isProficientInvestigation = table.Column<bool>(type: "bit", nullable: false),
                    isProficientMedecine = table.Column<bool>(type: "bit", nullable: false),
                    isProficientNature = table.Column<bool>(type: "bit", nullable: false),
                    isProficientPerception = table.Column<bool>(type: "bit", nullable: false),
                    isProficientPerformance = table.Column<bool>(type: "bit", nullable: false),
                    isProficientPersuasion = table.Column<bool>(type: "bit", nullable: false),
                    isProficientReligion = table.Column<bool>(type: "bit", nullable: false),
                    isProficientSleighOfHand = table.Column<bool>(type: "bit", nullable: false),
                    isProficientStealth = table.Column<bool>(type: "bit", nullable: false),
                    isProficientSurvival = table.Column<bool>(type: "bit", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CharacterId",
                table: "Skills",
                column: "CharacterId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
