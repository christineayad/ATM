using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ATMAPI.Migrations
{
    /// <inheritdoc />
    public partial class AllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OperationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserBalanceBeforeOperation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserBalanceAfterOperation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: true),
                    IsCompleteTransfer = table.Column<bool>(type: "bit", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currentbalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_UserCategories_UserCategoryId",
                        column: x => x.UserCategoryId,
                        principalTable: "UserCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserOperationHestories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    OperationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOperationHestories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOperationHestories_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserOperationHestories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTransactions_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OperationId",
                table: "Transactions",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationHestories_OperationId",
                table: "UserOperationHestories",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOperationHestories_UserId",
                table: "UserOperationHestories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserCategoryId",
                table: "Users",
                column: "UserCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTransactions_TransactionId",
                table: "UserTransactions",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTransactions_UserId",
                table: "UserTransactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOperationHestories");

            migrationBuilder.DropTable(
                name: "UserTransactions");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
