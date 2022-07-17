using Microsoft.EntityFrameworkCore.Migrations;

namespace BitcoinMarket.Migrations
{
    public partial class deleteRestrictionsChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartialTrades_Trades_BuyTradeId",
                table: "PartialTrades");

            migrationBuilder.DropForeignKey(
                name: "FK_PartialTrades_Trades_SellTradeId",
                table: "PartialTrades");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Users_TransactionOwnerId",
                table: "Trades");

            migrationBuilder.AddForeignKey(
                name: "FK_PartialTrades_Trades_BuyTradeId",
                table: "PartialTrades",
                column: "BuyTradeId",
                principalTable: "Trades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PartialTrades_Trades_SellTradeId",
                table: "PartialTrades",
                column: "SellTradeId",
                principalTable: "Trades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Users_TransactionOwnerId",
                table: "Trades",
                column: "TransactionOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartialTrades_Trades_BuyTradeId",
                table: "PartialTrades");

            migrationBuilder.DropForeignKey(
                name: "FK_PartialTrades_Trades_SellTradeId",
                table: "PartialTrades");

            migrationBuilder.DropForeignKey(
                name: "FK_Trades_Users_TransactionOwnerId",
                table: "Trades");

            migrationBuilder.AddForeignKey(
                name: "FK_PartialTrades_Trades_BuyTradeId",
                table: "PartialTrades",
                column: "BuyTradeId",
                principalTable: "Trades",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartialTrades_Trades_SellTradeId",
                table: "PartialTrades",
                column: "SellTradeId",
                principalTable: "Trades",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trades_Users_TransactionOwnerId",
                table: "Trades",
                column: "TransactionOwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
