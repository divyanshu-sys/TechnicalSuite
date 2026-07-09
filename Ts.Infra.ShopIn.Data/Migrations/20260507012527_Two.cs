using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ts.Infra.ShopIn.Data.Migrations
{
    /// <inheritdoc />
    public partial class Two : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetailViews_ProductDetails_ProductId",
                schema: "dbo",
                table: "ProductDetailViews");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                schema: "dbo",
                table: "ProductDetailViews",
                newName: "ProductDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetailViews_ProductDetails_ProductDetailId",
                schema: "dbo",
                table: "ProductDetailViews",
                column: "ProductDetailId",
                principalSchema: "dbo",
                principalTable: "ProductDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetailViews_ProductDetails_ProductDetailId",
                schema: "dbo",
                table: "ProductDetailViews");

            migrationBuilder.RenameColumn(
                name: "ProductDetailId",
                schema: "dbo",
                table: "ProductDetailViews",
                newName: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetailViews_ProductDetails_ProductId",
                schema: "dbo",
                table: "ProductDetailViews",
                column: "ProductId",
                principalSchema: "dbo",
                principalTable: "ProductDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
