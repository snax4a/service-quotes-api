using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceQuotes.Infrastructure.Migrations
{
    public partial class AddedAccountSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Created", "Email", "PasswordHash", "Role", "Updated" },
                values: new object[] { new Guid("7542b6b8-638c-44c9-806b-0040667c32a9"), new DateTime(2021, 3, 1, 22, 46, 58, 919, DateTimeKind.Utc).AddTicks(9540), "manager@service-quotes.com", "$2a$11$RtlK2XAwpq2cY0EOnZlVJOpcM7BKnTbUNy50tZ14D57Og8iZcP5pi", 0, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: new Guid("7542b6b8-638c-44c9-806b-0040667c32a9"));
        }
    }
}
