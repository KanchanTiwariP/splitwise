using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SplitWise.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseCreatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Expense",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Expense_CreatedBy",
                table: "Expense",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Users_CreatedBy",
                table: "Expense",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Users_CreatedBy",
                table: "Expense");

            migrationBuilder.DropIndex(
                name: "IX_Expense_CreatedBy",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Expense");
        }
    }
}
