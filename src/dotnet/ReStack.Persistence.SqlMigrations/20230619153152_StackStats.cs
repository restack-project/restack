using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReStack.Persistence.SqlMigrations
{
    /// <inheritdoc />
    public partial class StackStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AverageRuntime",
                table: "Stack",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SuccesPercentage",
                table: "Stack",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ended",
                table: "Job",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "Sequence",
                table: "Job",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Success",
                table: "Job",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TriggerBy",
                table: "Job",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRuntime",
                table: "Stack");

            migrationBuilder.DropColumn(
                name: "SuccesPercentage",
                table: "Stack");

            migrationBuilder.DropColumn(
                name: "Sequence",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "Success",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "TriggerBy",
                table: "Job");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Ended",
                table: "Job",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
