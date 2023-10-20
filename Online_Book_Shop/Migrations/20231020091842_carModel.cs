using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Book_Shop.Migrations
{
    /// <inheritdoc />
    public partial class carModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Cart_Bookid",
                table: "Cart",
                column: "Bookid");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserId",
                table: "Cart",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Books_Bookid",
                table: "Cart",
                column: "Bookid",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Users_UserId",
                table: "Cart",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Books_Bookid",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Users_UserId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_Bookid",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_UserId",
                table: "Cart");
        }
    }
}
