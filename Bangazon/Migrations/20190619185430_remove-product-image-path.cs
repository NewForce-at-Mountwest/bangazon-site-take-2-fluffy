using Microsoft.EntityFrameworkCore.Migrations;

namespace Bangazon.Migrations
{
    public partial class removeproductimagepath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Product");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7c096a14-2c34-49c2-9342-f40003057386", "AQAAAAEAACcQAAAAEBjnZPuAzEtWYDVqSsSxv636qAH/HUozYmlILg2cfOqjut1NRhQM5Rr1kfWUPmef6g==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Product",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "840f99f9-e9a7-4943-821c-934b64e9ede1", "AQAAAAEAACcQAAAAEHGYSjLc388ZPpZNy9iIHoN7j28OdJq4aYiC3CbUeYuwkz2HTAtpGh0sdvgR+JNFUA==" });
        }
    }
}
