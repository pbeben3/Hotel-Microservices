using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Reservations.Storage.Migrations
{
    /// <inheritdoc />
    public partial class AddedPriceBeforeDiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceBeforeDiscount",
                schema: "Reservation",
                table: "Reservations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceBeforeDiscount",
                schema: "Reservation",
                table: "Reservations");
        }
    }
}
