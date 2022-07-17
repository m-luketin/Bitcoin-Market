using Microsoft.EntityFrameworkCore.Migrations;

namespace BitcoinMarket.Migrations
{
    public partial class BrutalRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Users_BuyerId",
                table: "Trades");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Users_SellerId",
                table: "Trades");

            migrationBuilder.DropIndex(
                name: "IX_Trades_SellerId",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Trades");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "Trades",
                newName: "TransactionOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Trades_BuyerId",
                table: "Trades",
                newName: "IX_Trades_TransactionOwnerId");

            migrationBuilder.AlterColumn<decimal>(
                name: "BtcBalance",
                table: "Users",
                type: "decimal(20,10)",
                precision: 20,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ValueInBtc",
                table: "Trades",
                type: "decimal(20,10)",
                precision: 20,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,4)");

            migrationBuilder.AddColumn<decimal>(
                name: "FilledValue",
                table: "Trades",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsBuy",
                table: "Trades",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PartialTrades",
                columns: table => new
                {
                    PartialTradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellTradeId = table.Column<int>(type: "int", nullable: false),
                    BuyTradeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(12,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartialTrades", x => x.PartialTradeId);
                    table.ForeignKey(
                        name: "FK_PartialTrades_Trades_BuyTradeId",
                        column: x => x.BuyTradeId,
                        principalTable: "Trades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PartialTrades_Trades_SellTradeId",
                        column: x => x.SellTradeId,
                        principalTable: "Trades",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartialTrades_BuyTradeId",
                table: "PartialTrades",
                column: "BuyTradeId");

            migrationBuilder.CreateIndex(
                name: "IX_PartialTrades_SellTradeId",
                table: "PartialTrades",
                column: "SellTradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Users_TransactionOwnerId",
                table: "Trades",
                column: "TransactionOwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Users_TransactionOwnerId",
                table: "Trades");

            migrationBuilder.DropTable(
                name: "PartialTrades");

            migrationBuilder.DropColumn(
                name: "FilledValue",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "IsBuy",
                table: "Trades");

            migrationBuilder.RenameColumn(
                name: "TransactionOwnerId",
                table: "Trades",
                newName: "BuyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Trades_TransactionOwnerId",
                table: "Trades",
                newName: "IX_Trades_BuyerId");

            migrationBuilder.AlterColumn<decimal>(
                name: "BtcBalance",
                table: "Users",
                type: "decimal(12,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,10)",
                oldPrecision: 20,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValueInBtc",
                table: "Trades",
                type: "decimal(12,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,10)",
                oldPrecision: 20,
                oldScale: 10);

            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "Trades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Trades_SellerId",
                table: "Trades",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Users_BuyerId",
                table: "Trades",
                column: "BuyerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Users_SellerId",
                table: "Trades",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
