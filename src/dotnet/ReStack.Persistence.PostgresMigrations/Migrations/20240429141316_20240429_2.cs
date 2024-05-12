using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReStack.Persistence.PostgresMigrations.Migrations
{
    /// <inheritdoc />
    public partial class _20240429_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComponentName",
                table: "Log");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ComponentName",
                table: "Log",
                type: "text",
                nullable: true);
        }
    }
}
