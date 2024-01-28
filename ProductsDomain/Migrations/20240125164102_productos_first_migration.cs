using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsDomain.Migrations
{
    /// <inheritdoc />
    public partial class productos_first_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DotnetProductsPractice");

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "DotnetProductsPractice",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ean = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IVA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products",
                schema: "DotnetProductsPractice");
        }
    }
}
