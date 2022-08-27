using Microsoft.EntityFrameworkCore.Migrations;

namespace BitcoinMarket.Migrations
{
    public partial class deleteRestrictionsChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartialOrders_Orders_BuyOrderId",
                table: "PartialOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PartialOrders_Orders_SellOrderId",
                table: "PartialOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_TransactionOwnerId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_PartialOrders_Orders_BuyOrderId",
                table: "PartialOrders",
                column: "BuyOrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PartialOrders_Orders_SellOrderId",
                table: "PartialOrders",
                column: "SellOrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_TransactionOwnerId",
                table: "Orders",
                column: "TransactionOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartialOrders_Orders_BuyOrderId",
                table: "PartialOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PartialOrders_Orders_SellOrderId",
                table: "PartialOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_TransactionOwnerId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_PartialOrders_Orders_BuyOrderId",
                table: "PartialOrders",
                column: "BuyOrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartialOrders_Orders_SellOrderId",
                table: "PartialOrders",
                column: "SellOrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_TransactionOwnerId",
                table: "Orders",
                column: "TransactionOwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
