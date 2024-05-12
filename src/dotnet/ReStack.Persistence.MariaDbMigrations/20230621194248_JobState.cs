using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReStack.Persistence.MariaDbMigrations
{
    /// <inheritdoc />
    public partial class JobState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Success",
                table: "Job");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Job",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Job");

            migrationBuilder.AddColumn<bool>(
                name: "Success",
                table: "Job",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
