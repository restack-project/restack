using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReStack.Persistence.MariaDbMigrations
{
    /// <inheritdoc />
    public partial class LibraryCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastHashCommit",
                table: "ComponentLibrary",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastHashCommit",
                table: "ComponentLibrary");
        }
    }
}
