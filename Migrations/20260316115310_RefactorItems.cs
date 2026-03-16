using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Whizsheet.Api.Migrations
{
    /// <inheritdoc />
    public partial class RefactorItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterItems");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MagicItemEffects");

            migrationBuilder.RenameColumn(
                name: "Discriminator",
                table: "Items",
                newName: "ItemType");

            migrationBuilder.AddColumn<int>(
                name: "ChargesRemaining",
                table: "MagicItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MagicEffectDescription",
                table: "MagicItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MagicEffectMechanics",
                table: "MagicItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MagicRechargeRate",
                table: "MagicItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalCharges",
                table: "MagicItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CharacterId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAttuned",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEquipped",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Items_CharacterId",
                table: "Items",
                column: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Characters_CharacterId",
                table: "Items",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Characters_CharacterId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_CharacterId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ChargesRemaining",
                table: "MagicItems");

            migrationBuilder.DropColumn(
                name: "MagicEffectDescription",
                table: "MagicItems");

            migrationBuilder.DropColumn(
                name: "MagicEffectMechanics",
                table: "MagicItems");

            migrationBuilder.DropColumn(
                name: "MagicRechargeRate",
                table: "MagicItems");

            migrationBuilder.DropColumn(
                name: "TotalCharges",
                table: "MagicItems");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsAttuned",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "IsEquipped",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ItemType",
                table: "Items",
                newName: "Discriminator");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MagicItemEffects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CharacterItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CharacterId1 = table.Column<int>(type: "int", nullable: true),
                    ChargesRemaining = table.Column<int>(type: "int", nullable: true),
                    IsAttuned = table.Column<bool>(type: "bit", nullable: false),
                    IsEquipped = table.Column<bool>(type: "bit", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
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
        }
    }
}
