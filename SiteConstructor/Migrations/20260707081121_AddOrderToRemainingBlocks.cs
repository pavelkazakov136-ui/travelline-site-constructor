using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiteConstructor.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderToRemainingBlocks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Vacancies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "GalleryItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Clients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Bonuses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Vacancies");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "GalleryItems");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Bonuses");
        }
    }
}
