using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceQuotes.Infrastructure.Migrations
{
    public partial class AddedEmployeeEntityRelatedToAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "AccountId", "FirstName", "LastName" },
                values: new object[] { new Guid("5e02401f-bf8c-4e2f-b4a8-a7e27cd3678d"), new Guid("7542b6b8-638c-44c9-806b-0040667c32a9"), "Szymon", "Sus" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("5e02401f-bf8c-4e2f-b4a8-a7e27cd3678d"));
        }
    }
}
