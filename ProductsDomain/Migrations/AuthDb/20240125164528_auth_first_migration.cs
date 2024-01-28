using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsDomain.Migrations.AuthDb
{
    /// <inheritdoc />
    public partial class auth_first_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DotnetProductsPractice");

            migrationBuilder.CreateTable(
                name: "Auth",
                schema: "DotnetProductsPractice",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth", x => x.Email);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auth",
                schema: "DotnetProductsPractice");
        }
    }
}
