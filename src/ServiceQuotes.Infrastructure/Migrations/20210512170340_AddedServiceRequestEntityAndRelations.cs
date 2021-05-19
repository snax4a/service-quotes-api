using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceQuotes.Infrastructure.Migrations
{
    public partial class AddedServiceRequestEntityAndRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PlannedExecutionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AddressId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_CustomerAddresses_CustomerId_AddressId",
                        columns: x => new { x.CustomerId, x.AddressId },
                        principalTable: "CustomerAddresses",
                        principalColumns: new[] { "CustomerId", "AddressId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequestEmployee",
                columns: table => new
                {
                    ServiceRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequestEmployee", x => new { x.ServiceRequestId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_ServiceRequestEmployee_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRequestEmployee_ServiceRequests_ServiceRequestId",
                        column: x => x.ServiceRequestId,
                        principalTable: "ServiceRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestEmployee_EmployeeId",
                table: "ServiceRequestEmployee",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_CustomerId_AddressId",
                table: "ServiceRequests",
                columns: new[] { "CustomerId", "AddressId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceRequestEmployee");

            migrationBuilder.DropTable(
                name: "ServiceRequests");
        }
    }
}
