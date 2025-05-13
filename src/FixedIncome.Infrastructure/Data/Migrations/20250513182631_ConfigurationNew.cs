using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixedIncome.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurationNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvestimentTitle",
                schema: "fixed_incomes",
                table: "fixed_income_simulation",
                newName: "InvestmentTitle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvestmentTitle",
                schema: "fixed_incomes",
                table: "fixed_income_simulation",
                newName: "InvestimentTitle");
        }
    }
}
