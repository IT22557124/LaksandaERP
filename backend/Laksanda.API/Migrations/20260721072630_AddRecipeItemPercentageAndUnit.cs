using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laksanda.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeItemPercentageAndUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "RecipeItems",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "RecipeItems",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "RecipeItems");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "RecipeItems");
        }
    }
}
