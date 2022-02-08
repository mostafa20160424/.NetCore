using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeeManagement.Migrations
{
    public partial class SeedingData2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "Department", "Email", "Name" },
                values: new object[] { 2, 2, "khaled@yahoo.com", "khaled" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "Department", "Email", "Name" },
                values: new object[] { 3, 2, "abdalla@yahoo.com", "abdalla" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "Department", "Email", "Name" },
                values: new object[] { 1, 2, "mostafa@yahoo.com", "mostafa" });
        }
    }
}
