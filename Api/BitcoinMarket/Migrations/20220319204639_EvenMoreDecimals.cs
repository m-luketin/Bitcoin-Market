using Microsoft.EntityFrameworkCore.Migrations;

namespace BitcoinMarket.Migrations
{
    public partial class EvenMoreDecimals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BtcBalance",
                table: "Users",
                type: "decimal(20,10)",
                precision: 20,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,6)",
                oldPrecision: 12,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValueInBtc",
                table: "Trades",
                type: "decimal(20,10)",
                precision: 20,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,6)",
                oldPrecision: 12,
                oldScale: 6);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BtcBalance",
                table: "Users",
                type: "decimal(12,6)",
                precision: 12,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,10)",
                oldPrecision: 20,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValueInBtc",
                table: "Trades",
                type: "decimal(12,6)",
                precision: 12,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,10)",
                oldPrecision: 20,
                oldScale: 10);
        }
    }
}
