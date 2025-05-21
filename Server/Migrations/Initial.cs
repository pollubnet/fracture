#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace Fracture.Server.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "Users",
            table => new
            {
                Id = table
                    .Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Username = table.Column<string>("TEXT", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            "Items",
            table => new
            {
                Id = table
                    .Column<int>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>("TEXT", nullable: false),
                History = table.Column<string>("TEXT", nullable: true),
                Rarity = table.Column<int>("INTEGER", nullable: false),
                Type = table.Column<int>("INTEGER", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false),
                IsEquipped = table.Column<bool>("INTEGER", nullable: false),
                CreatedById = table.Column<int>("INTEGER", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Items", x => x.Id);
                table.ForeignKey(
                    "FK_Items_Users_CreatedById",
                    x => x.CreatedById,
                    "Users",
                    "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateTable(
            "ItemStatistics",
            table => new
            {
                ItemId = table.Column<int>("INTEGER", nullable: false),
                Luck = table.Column<int>("INTEGER", nullable: false),
                Health = table.Column<int>("INTEGER", nullable: false),
                Armor = table.Column<int>("INTEGER", nullable: false),
                Power = table.Column<int>("INTEGER", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ItemStatistics", x => x.ItemId);
                table.ForeignKey(
                    "FK_ItemStatistics_Items_ItemId",
                    x => x.ItemId,
                    "Items",
                    "Id",
                    onDelete: ReferentialAction.Cascade
                );
            }
        );

        migrationBuilder.CreateIndex("IX_Items_CreatedById", "Items", "CreatedById");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("ItemStatistics");

        migrationBuilder.DropTable("Items");

        migrationBuilder.DropTable("Users");
    }
}
