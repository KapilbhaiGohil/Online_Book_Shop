using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_Book_Shop.Migrations
{
    /// <inheritdoc />
    public partial class purchaseModelUpdattion3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PurchaseBook_BookId",
                table: "PurchaseBook",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseBook_PurchaseId",
                table: "PurchaseBook",
                column: "PurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseBook_Books_BookId",
                table: "PurchaseBook",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseBook_Purchase_PurchaseId",
                table: "PurchaseBook",
                column: "PurchaseId",
                principalTable: "Purchase",
                principalColumn: "PurchaseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseBook_Books_BookId",
                table: "PurchaseBook");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseBook_Purchase_PurchaseId",
                table: "PurchaseBook");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseBook_BookId",
                table: "PurchaseBook");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseBook_PurchaseId",
                table: "PurchaseBook");
        }
    }
}
