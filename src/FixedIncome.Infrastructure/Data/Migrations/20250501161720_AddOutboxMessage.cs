using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FixedIncome.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FixedIncomeOrderId1",
                schema: "fixed_incomes",
                table: "fixed_income_order_event",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FixedIncomeSimId",
                schema: "fixed_incomes",
                table: "fixed_income_order",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "outbox_message",
                schema: "fixed_incomes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    OccuredOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    Content = table.Column<string>(type: "jsonb", nullable: false),
                    ProcessedOn = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_message", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fixed_income_order_event_FixedIncomeOrderId1",
                schema: "fixed_incomes",
                table: "fixed_income_order_event",
                column: "FixedIncomeOrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_fixed_income_order_FixedIncomeSimId",
                schema: "fixed_incomes",
                table: "fixed_income_order",
                column: "FixedIncomeSimId");

            migrationBuilder.AddForeignKey(
                name: "FK_fixed_income_order_fixed_income_simulation_FixedIncomeSimId",
                schema: "fixed_incomes",
                table: "fixed_income_order",
                column: "FixedIncomeSimId",
                principalSchema: "fixed_incomes",
                principalTable: "fixed_income_simulation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_fixed_income_order_event_fixed_income_order_FixedIncomeOrd~1",
                schema: "fixed_incomes",
                table: "fixed_income_order_event",
                column: "FixedIncomeOrderId1",
                principalSchema: "fixed_incomes",
                principalTable: "fixed_income_order",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fixed_income_order_fixed_income_simulation_FixedIncomeSimId",
                schema: "fixed_incomes",
                table: "fixed_income_order");

            migrationBuilder.DropForeignKey(
                name: "FK_fixed_income_order_event_fixed_income_order_FixedIncomeOrd~1",
                schema: "fixed_incomes",
                table: "fixed_income_order_event");

            migrationBuilder.DropTable(
                name: "outbox_message",
                schema: "fixed_incomes");

            migrationBuilder.DropIndex(
                name: "IX_fixed_income_order_event_FixedIncomeOrderId1",
                schema: "fixed_incomes",
                table: "fixed_income_order_event");

            migrationBuilder.DropIndex(
                name: "IX_fixed_income_order_FixedIncomeSimId",
                schema: "fixed_incomes",
                table: "fixed_income_order");

            migrationBuilder.DropColumn(
                name: "FixedIncomeOrderId1",
                schema: "fixed_incomes",
                table: "fixed_income_order_event");

            migrationBuilder.DropColumn(
                name: "FixedIncomeSimId",
                schema: "fixed_incomes",
                table: "fixed_income_order");
        }
    }
}
