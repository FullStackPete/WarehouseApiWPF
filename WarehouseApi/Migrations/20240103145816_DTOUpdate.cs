using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WarehouseApi.Migrations
{
    /// <inheritdoc />
    public partial class DTOUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Secret",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Secret",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Secret",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Secret",
                table: "Categories");
        }
    }
}
