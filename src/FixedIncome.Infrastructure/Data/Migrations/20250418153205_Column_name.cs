using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixedIncome.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Column_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalAmount",
                schema: "fixed_incomes",
                table: "fixed_income_simulation");

            migrationBuilder.DropColumn(
                name: "FinalAmountNet",
                schema: "fixed_incomes",
                table: "fixed_income_simulation");

            migrationBuilder.AddColumn<decimal>(
                name: "FinalGrossAmount",
                schema: "fixed_incomes",
                table: "fixed_income_simulation",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalNetAmount",
                schema: "fixed_incomes",
                table: "fixed_income_simulation",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalGrossAmount",
                schema: "fixed_incomes",
                table: "fixed_income_simulation");

            migrationBuilder.DropColumn(
                name: "FinalNetAmount",
                schema: "fixed_incomes",
                table: "fixed_income_simulation");

            migrationBuilder.AddColumn<decimal>(
                name: "FinalAmount",
                schema: "fixed_incomes",
                table: "fixed_income_simulation",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalAmountNet",
                schema: "fixed_incomes",
                table: "fixed_income_simulation",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
