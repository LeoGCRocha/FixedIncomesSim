using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixedIncome.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Configuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_fixed_income_simulation_Information_Type",
                schema: "fixed_incomes",
                table: "fixed_income_simulation");

            migrationBuilder.RenameColumn(
                name: "investiment_title",
                schema: "fixed_incomes",
                table: "fixed_income_simulation",
                newName: "InvestimentTitle");

            migrationBuilder.RenameColumn(
                name: "Information_Type",
                schema: "fixed_incomes",
                table: "fixed_income_simulation",
                newName: "InformationType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvestimentTitle",
                schema: "fixed_incomes",
                table: "fixed_income_simulation",
                newName: "investiment_title");

            migrationBuilder.RenameColumn(
                name: "InformationType",
                schema: "fixed_incomes",
                table: "fixed_income_simulation",
                newName: "Information_Type");

            migrationBuilder.CreateIndex(
                name: "IX_fixed_income_simulation_Information_Type",
                schema: "fixed_incomes",
                table: "fixed_income_simulation",
                column: "Information_Type");
        }
    }
}
