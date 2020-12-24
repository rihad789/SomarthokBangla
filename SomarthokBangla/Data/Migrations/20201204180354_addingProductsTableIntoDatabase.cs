using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SomarthokBangla.Data.Migrations
{
    public partial class addingProductsTableIntoDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
            name: "Products",
        columns: table => new
        {
            Id = table.Column<int>(nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            DetailsHtmlLink = table.Column<string>(nullable: true),
            Image = table.Column<string>(nullable: true),
            IsAvailable = table.Column<bool>(nullable: false),
            ManufacturerId = table.Column<int>(nullable: false),
            ModelNo = table.Column<string>(nullable: true),
            Name = table.Column<string>(nullable: false),
            Price = table.Column<decimal>(nullable: false),
            Quantity = table.Column<decimal>(nullable: false),           
            ProductTypeId = table.Column<int>(nullable: false),
            UplodaDate = table.Column<DateTime>(nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_Products", x => x.Id);
            table.ForeignKey(
                name: "FK_Products_Manufacturer_ManufacturerId",
                column: x => x.ManufacturerId,
                principalTable: "Manufacturer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                name: "FK_Products_ProductTypes_ProductTypeId",
                column: x => x.ProductTypeId,
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

        });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ManufacturerId",
                table: "Products",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeId",
                table: "Products",
                column: "ProductTypeId");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
            name: "Products");

        }
    }
}
