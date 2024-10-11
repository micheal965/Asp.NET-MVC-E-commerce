using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Franshy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Reseeddatabasetablesid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DBCC CHECKIDENT ('Categories', RESEED, 0);");
            migrationBuilder.Sql("DBCC CHECKIDENT ('Products', RESEED, 0);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
