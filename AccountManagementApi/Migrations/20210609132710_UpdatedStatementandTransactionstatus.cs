using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountManagementApi.Migrations
{
    public partial class UpdatedStatementandTransactionstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionStatus_Account_AccountID",
                table: "TransactionStatus");

            migrationBuilder.DropIndex(
                name: "IX_TransactionStatus_AccountID",
                table: "TransactionStatus");

            migrationBuilder.DropColumn(
                name: "AccountID",
                table: "TransactionStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountID",
                table: "TransactionStatus",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStatus_AccountID",
                table: "TransactionStatus",
                column: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionStatus_Account_AccountID",
                table: "TransactionStatus",
                column: "AccountID",
                principalTable: "Account",
                principalColumn: "AccountID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
