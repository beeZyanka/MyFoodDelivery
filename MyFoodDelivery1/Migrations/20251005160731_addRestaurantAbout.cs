using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFoodDelivery1.Migrations
{
    /// <inheritdoc />
    public partial class addRestaurantAbout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "Restaurants");
        }
    }
}
