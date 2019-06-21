using Microsoft.EntityFrameworkCore.Migrations;

namespace Bangazon.Migrations
{
    public partial class localdelivery : Migration
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
                values: new object[] { "32469afe-fb42-477a-b8ce-a3fd9c5c70c9", "AQAAAAEAACcQAAAAEN8Un92HNapZPaado6hzbnil8IIIBKZSmVLmqf4zHFpyRB8Ug8ss4MDDVxKWidB2Rw==" });
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
