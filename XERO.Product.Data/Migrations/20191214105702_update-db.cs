using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Xero.Product.Data.Migrations
{
    public partial class updatedb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOption_Product_ProductId",
                table: "ProductOption");

            migrationBuilder.DropIndex(
                name: "IX_ProductOption_ProductId",
                table: "ProductOption");

            migrationBuilder.DropColumn(
                name: "test",
                table: "Product");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductDataId",
                table: "ProductOption",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOption_ProductDataId",
                table: "ProductOption",
                column: "ProductDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOption_Product_ProductDataId",
                table: "ProductOption",
                column: "ProductDataId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOption_Product_ProductDataId",
                table: "ProductOption");

            migrationBuilder.DropIndex(
                name: "IX_ProductOption_ProductDataId",
                table: "ProductOption");

            migrationBuilder.DropColumn(
                name: "ProductDataId",
                table: "ProductOption");

            migrationBuilder.AddColumn<int>(
                name: "test",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOption_ProductId",
                table: "ProductOption",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOption_Product_ProductId",
                table: "ProductOption",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
