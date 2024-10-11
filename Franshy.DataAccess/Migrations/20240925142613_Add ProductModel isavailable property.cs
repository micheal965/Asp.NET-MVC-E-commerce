using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Franshy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductModelisavailableproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Isavailable",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isavailable",
                table: "Products");
        }
    }
}
