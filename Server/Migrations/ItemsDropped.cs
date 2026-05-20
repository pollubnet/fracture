using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fracture.Server.Migrations
{
    /// <inheritdoc />
    public partial class ItemsDropped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Users_CreatedById",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Items",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateTable(
                name: "ItemsDropped",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    MapSeed = table.Column<int>(type: "INTEGER", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsDropped", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemsDropped_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemsDropped_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemsDropped_ItemId",
                table: "ItemsDropped",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsDropped_UserId_MapSeed_X_Y",
                table: "ItemsDropped",
                columns: new[] { "UserId", "MapSeed", "X", "Y" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Users_CreatedById",
                table: "Items",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Users_CreatedById",
                table: "Items");

            migrationBuilder.DropTable(
                name: "ItemsDropped");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Users_CreatedById",
                table: "Items",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
