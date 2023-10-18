using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Book_Shop.Migrations
{
    /// <inheritdoc />
    public partial class updateBookModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "Dimension",
                table: "Books",
                newName: "Width");

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "Books",
                newName: "Dimension");

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
