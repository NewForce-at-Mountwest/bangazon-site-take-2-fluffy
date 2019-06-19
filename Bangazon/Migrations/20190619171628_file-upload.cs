using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bangazon.Migrations
{
    public partial class fileupload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProductImage",
                table: "Product",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "840f99f9-e9a7-4943-821c-934b64e9ede1", "AQAAAAEAACcQAAAAEHGYSjLc388ZPpZNy9iIHoN7j28OdJq4aYiC3CbUeYuwkz2HTAtpGh0sdvgR+JNFUA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImage",
                table: "Product");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9d8b9c57-ce47-42a0-b558-9ef37f3daa39", "AQAAAAEAACcQAAAAEMF8OqH9cXEugrP9C+oBLv6Fu72ASq2GhkrWrLgaQGpavt6UrXTaXxwzqm3Cl3T5vw==" });
        }
    }
}
