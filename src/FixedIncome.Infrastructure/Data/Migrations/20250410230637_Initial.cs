using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixedIncome.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "fixed_incomes");

            migrationBuilder.CreateTable(
                name: "fixed_income_simulation",
                schema: "fixed_incomes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartAmount = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    MonthlyYield = table.Column<decimal>(type: "numeric", nullable: false),
                    MonthlyContribution = table.Column<decimal>(type: "numeric", nullable: false),
                    InvestedAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    FinalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    FinalAmountNet = table.Column<decimal>(type: "numeric", nullable: false),
                    investiment_title = table.Column<string>(type: "text", nullable: false),
                    Information_Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fixed_income_simulation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fixed_income_balance",
                schema: "fixed_incomes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Profit = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    FixedIncomeId = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fixed_income_balance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_fixed_income_balance_fixed_income_simulation_FixedIncomeId",
                        column: x => x.FixedIncomeId,
                        principalSchema: "fixed_incomes",
                        principalTable: "fixed_income_simulation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fixed_income_order",
                schema: "fixed_incomes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartAmount = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    CurrentAmount = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    Tax = table.Column<decimal>(type: "numeric", nullable: false),
                    TaxGroup = table.Column<int>(type: "integer", nullable: false),
                    MonthlyYield = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    FixedIncomeId = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fixed_income_order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_fixed_income_order_fixed_income_simulation_FixedIncomeId",
                        column: x => x.FixedIncomeId,
                        principalSchema: "fixed_incomes",
                        principalTable: "fixed_income_simulation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fixed_income_order_event",
                schema: "fixed_incomes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartReferenceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndReferenceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    MonthlyYield = table.Column<decimal>(type: "numeric", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    Profit = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    FixedIncomeId = table.Column<Guid>(type: "uuid", nullable: true),
                    FixedIncomeOrderId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fixed_income_order_event", x => x.Id);
                    table.ForeignKey(
                        name: "FK_fixed_income_order_event_fixed_income_order_FixedIncomeId",
                        column: x => x.FixedIncomeId,
                        principalSchema: "fixed_incomes",
                        principalTable: "fixed_income_order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fixed_income_order_event_fixed_income_order_FixedIncomeOrde~",
                        column: x => x.FixedIncomeOrderId,
                        principalSchema: "fixed_incomes",
                        principalTable: "fixed_income_order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fixed_income_balance_FixedIncomeId",
                schema: "fixed_incomes",
                table: "fixed_income_balance",
                column: "FixedIncomeId");

            migrationBuilder.CreateIndex(
                name: "IX_fixed_income_order_FixedIncomeId",
                schema: "fixed_incomes",
                table: "fixed_income_order",
                column: "FixedIncomeId");

            migrationBuilder.CreateIndex(
                name: "IX_fixed_income_order_event_FixedIncomeId",
                schema: "fixed_incomes",
                table: "fixed_income_order_event",
                column: "FixedIncomeId");

            migrationBuilder.CreateIndex(
                name: "IX_fixed_income_order_event_FixedIncomeOrderId",
                schema: "fixed_incomes",
                table: "fixed_income_order_event",
                column: "FixedIncomeOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_fixed_income_simulation_CreatedAt",
                schema: "fixed_incomes",
                table: "fixed_income_simulation",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_fixed_income_simulation_Information_Type",
                schema: "fixed_incomes",
                table: "fixed_income_simulation",
                column: "Information_Type");

            migrationBuilder.CreateIndex(
                name: "IX_fixed_income_simulation_StartDate",
                schema: "fixed_incomes",
                table: "fixed_income_simulation",
                column: "StartDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fixed_income_balance",
                schema: "fixed_incomes");

            migrationBuilder.DropTable(
                name: "fixed_income_order_event",
                schema: "fixed_incomes");

            migrationBuilder.DropTable(
                name: "fixed_income_order",
                schema: "fixed_incomes");

            migrationBuilder.DropTable(
                name: "fixed_income_simulation",
                schema: "fixed_incomes");
        }
    }
}
