using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Book_Shop.Migrations
{
    /// <inheritdoc />
    public partial class UpdationInBookModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Books_BookId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Reviews",
                newName: "bookId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_BookId",
                table: "Reviews",
                newName: "IX_Reviews_bookId");

            migrationBuilder.RenameColumn(
                name: "isbn",
                table: "Books",
                newName: "ISBN");

            migrationBuilder.RenameIndex(
                name: "IX_Books_isbn",
                table: "Books",
                newName: "IX_Books_ISBN");

            migrationBuilder.AlterColumn<int>(
                name: "bookId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Books_bookId",
                table: "Reviews",
                column: "bookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Books_bookId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "bookId",
                table: "Reviews",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_bookId",
                table: "Reviews",
                newName: "IX_Reviews_BookId");

            migrationBuilder.RenameColumn(
                name: "ISBN",
                table: "Books",
                newName: "isbn");

            migrationBuilder.RenameIndex(
                name: "IX_Books_ISBN",
                table: "Books",
                newName: "IX_Books_isbn");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "Reviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Books_BookId",
                table: "Reviews",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}
