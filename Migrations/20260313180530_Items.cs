using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whizsheet.Api.Migrations
{
    /// <inheritdoc />
    public partial class Items : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemRarity = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    AttackType = table.Column<int>(type: "int", nullable: true),
                    BonusAttackRollType = table.Column<int>(type: "int", nullable: true),
                    DamageDiceType = table.Column<int>(type: "int", nullable: true),
                    DamageType = table.Column<int>(type: "int", nullable: true),
                    RangeType = table.Column<int>(type: "int", nullable: true),
                    IsLight = table.Column<bool>(type: "bit", nullable: true),
                    IsFinesse = table.Column<bool>(type: "bit", nullable: true),
                    IsThrown = table.Column<bool>(type: "bit", nullable: true),
                    IsVersatile = table.Column<bool>(type: "bit", nullable: true),
                    IsAmmunition = table.Column<bool>(type: "bit", nullable: true),
                    IsHeavy = table.Column<bool>(type: "bit", nullable: true),
                    IsReach = table.Column<bool>(type: "bit", nullable: true),
                    IsTwoHanded = table.Column<bool>(type: "bit", nullable: true),
                    IsLoading = table.Column<bool>(type: "bit", nullable: true),
                    IsSpecial = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsEquipped = table.Column<bool>(type: "bit", nullable: false),
                    IsAttuned = table.Column<bool>(type: "bit", nullable: false),
                    ChargesRemaining = table.Column<int>(type: "int", nullable: true),
                    CharacterId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterItems_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterItems_Characters_CharacterId1",
                        column: x => x.CharacterId1,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CharacterItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MagicItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequiresAttunement = table.Column<bool>(type: "bit", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagicItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MagicItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MagicItemEffects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EffectType = table.Column<int>(type: "int", nullable: true),
                    AbilityScore = table.Column<int>(type: "int", nullable: true),
                    SavingThrow = table.Column<int>(type: "int", nullable: true),
                    Skill = table.Column<int>(type: "int", nullable: true),
                    Modifier = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MagicItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagicItemEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MagicItemEffects_MagicItems_MagicItemId",
                        column: x => x.MagicItemId,
                        principalTable: "MagicItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterItems_CharacterId",
                table: "CharacterItems",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterItems_CharacterId1",
                table: "CharacterItems",
                column: "CharacterId1");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterItems_ItemId",
                table: "CharacterItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MagicItemEffects_MagicItemId",
                table: "MagicItemEffects",
                column: "MagicItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MagicItems_ItemId",
                table: "MagicItems",
                column: "ItemId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterItems");

            migrationBuilder.DropTable(
                name: "MagicItemEffects");

            migrationBuilder.DropTable(
                name: "MagicItems");

            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
