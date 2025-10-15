using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFoodDelivery1.Migrations
{
    /// <inheritdoc />
    public partial class AddRestaurantRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Restaurants");
        }
    }
}
