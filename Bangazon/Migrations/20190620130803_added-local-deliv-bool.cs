using Microsoft.EntityFrameworkCore.Migrations;

namespace Bangazon.Migrations
{
    public partial class addedlocaldelivbool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LocalDelivery",
                table: "Product",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "cf956d53-66a7-4d48-80e2-778f5ef0c9eb", "AQAAAAEAACcQAAAAEFr9KLESfXBo+vpLV00L2/2EIl5AUp0NAq0rTpeGdp3kdkqYdHCQGgkS0qTV8oC1jQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalDelivery",
                table: "Product");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7c096a14-2c34-49c2-9342-f40003057386", "AQAAAAEAACcQAAAAEBjnZPuAzEtWYDVqSsSxv636qAH/HUozYmlILg2cfOqjut1NRhQM5Rr1kfWUPmef6g==" });
        }
    }
}
