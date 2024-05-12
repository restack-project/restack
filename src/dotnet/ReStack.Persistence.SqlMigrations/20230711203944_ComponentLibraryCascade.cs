using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReStack.Persistence.SqlMigrations
{
    /// <inheritdoc />
    public partial class ComponentLibraryCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StackComponent_Component_ComponentId",
                table: "StackComponent");

            migrationBuilder.AddForeignKey(
                name: "FK_StackComponent_Component_ComponentId",
                table: "StackComponent",
                column: "ComponentId",
                principalTable: "Component",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StackComponent_Component_ComponentId",
                table: "StackComponent");

            migrationBuilder.AddForeignKey(
                name: "FK_StackComponent_Component_ComponentId",
                table: "StackComponent",
                column: "ComponentId",
                principalTable: "Component",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
