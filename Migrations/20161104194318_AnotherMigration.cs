using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheWallWIdentity.Migrations
{
    public partial class AnotherMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoppingcarts_Products_ProductId",
                table: "Shoppingcarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                table: "Products",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoppingcarts_products_ProductId",
                table: "Shoppingcarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoppingcarts_products_ProductId",
                table: "Shoppingcarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                table: "products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "products",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoppingcarts_Products_ProductId",
                table: "Shoppingcarts",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameTable(
                name: "products",
                newName: "Products");
        }
    }
}
