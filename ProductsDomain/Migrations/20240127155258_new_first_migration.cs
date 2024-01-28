using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsDomain.Migrations
{
    /// <inheritdoc />
    public partial class new_first_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ProductsSchema");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "DotnetProductsPractice",
                newName: "Products",
                newSchema: "ProductsSchema");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DotnetProductsPractice");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "ProductsSchema",
                newName: "Products",
                newSchema: "DotnetProductsPractice");
        }
    }
}
