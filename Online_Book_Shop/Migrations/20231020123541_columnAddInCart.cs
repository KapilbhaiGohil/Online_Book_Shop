using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Book_Shop.Migrations
{
    /// <inheritdoc />
    public partial class columnAddInCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Books_Bookid",
                table: "Cart");

            migrationBuilder.RenameColumn(
                name: "Bookid",
                table: "Cart",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_Bookid",
                table: "Cart",
                newName: "IX_Cart_BookId");

            migrationBuilder.AddColumn<int>(
                name: "Quentity",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Books_BookId",
                table: "Cart",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Books_BookId",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "Quentity",
                table: "Cart");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Cart",
                newName: "Bookid");

            migrationBuilder.RenameIndex(
                name: "IX_Cart_BookId",
                table: "Cart",
                newName: "IX_Cart_Bookid");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Books_Bookid",
                table: "Cart",
                column: "Bookid",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
