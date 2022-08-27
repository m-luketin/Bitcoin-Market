using Microsoft.EntityFrameworkCore.Migrations;

namespace BitcoinMarket.Migrations
{
    public partial class BrutalRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_BuyerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_SellerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SellerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "Orders",
                newName: "TransactionOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_BuyerId",
                table: "Orders",
                newName: "IX_Orders_TransactionOwnerId");

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
                table: "Orders",
                type: "decimal(20,10)",
                precision: 20,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,4)");

            migrationBuilder.AddColumn<decimal>(
                name: "FilledValue",
                table: "Orders",
                type: "decimal(12,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsBuy",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PartialOrders",
                columns: table => new
                {
                    PartialOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellOrderId = table.Column<int>(type: "int", nullable: false),
                    BuyOrderId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(12,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartialOrders", x => x.PartialOrderId);
                    table.ForeignKey(
                        name: "FK_PartialOrders_Orders_BuyOrderId",
                        column: x => x.BuyOrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PartialOrders_Orders_SellOrderId",
                        column: x => x.SellOrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartialOrders_BuyOrderId",
                table: "PartialOrders",
                column: "BuyOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PartialOrders_SellOrderId",
                table: "PartialOrders",
                column: "SellOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_TransactionOwnerId",
                table: "Orders",
                column: "TransactionOwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_TransactionOwnerId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "PartialOrders");

            migrationBuilder.DropColumn(
                name: "FilledValue",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsBuy",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "TransactionOwnerId",
                table: "Orders",
                newName: "BuyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_TransactionOwnerId",
                table: "Orders",
                newName: "IX_Orders_BuyerId");

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
                table: "Orders",
                type: "decimal(12,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,10)",
                oldPrecision: 20,
                oldScale: 10);

            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SellerId",
                table: "Orders",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_BuyerId",
                table: "Orders",
                column: "BuyerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_SellerId",
                table: "Orders",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
