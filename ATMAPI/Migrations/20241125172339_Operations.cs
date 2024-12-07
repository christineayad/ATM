using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ATMAPI.Migrations
{
    /// <inheritdoc />
    public partial class Operations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "OperationName" },
                values: new object[,]
                {
                    { 1, "Register" },
                    { 2, "Login" },
                    { 3, "CheckBalance" },
                    { 4, "Deposit" },
                    { 5, "Withdraw" },
                    { 6, "TransferMoney" },
                    { 7, "RecieveMoney" },
                    { 8, "RemoveUser" },
                    { 9, "LogOut" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operations");
        }
    }
}
