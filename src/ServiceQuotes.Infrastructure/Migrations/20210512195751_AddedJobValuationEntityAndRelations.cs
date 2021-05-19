using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceQuotes.Infrastructure.Migrations
{
    public partial class AddedJobValuationEntityAndRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_ServiceRequests_ServiceRequestId",
                table: "Material");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Material",
                table: "Material");

            migrationBuilder.RenameTable(
                name: "Material",
                newName: "Materials");

            migrationBuilder.RenameIndex(
                name: "IX_Material_ServiceRequestId",
                table: "Materials",
                newName: "IX_Materials_ServiceRequestId");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "Materials",
                type: "numeric(7,2)",
                precision: 7,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Materials",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Materials",
                table: "Materials",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "JobValuations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HourlyRate = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    LaborHours = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobValuations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequestJobValuations",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobValuationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequestJobValuations", x => new { x.EmployeeId, x.JobValuationId, x.ServiceRequestId });
                    table.ForeignKey(
                        name: "FK_ServiceRequestJobValuations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRequestJobValuations_JobValuations_JobValuationId",
                        column: x => x.JobValuationId,
                        principalTable: "JobValuations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRequestJobValuations_ServiceRequests_ServiceRequestId",
                        column: x => x.ServiceRequestId,
                        principalTable: "ServiceRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestJobValuations_JobValuationId",
                table: "ServiceRequestJobValuations",
                column: "JobValuationId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestJobValuations_ServiceRequestId",
                table: "ServiceRequestJobValuations",
                column: "ServiceRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_ServiceRequests_ServiceRequestId",
                table: "Materials",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_ServiceRequests_ServiceRequestId",
                table: "Materials");

            migrationBuilder.DropTable(
                name: "ServiceRequestJobValuations");

            migrationBuilder.DropTable(
                name: "JobValuations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Materials",
                table: "Materials");

            migrationBuilder.RenameTable(
                name: "Materials",
                newName: "Material");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_ServiceRequestId",
                table: "Material",
                newName: "IX_Material_ServiceRequestId");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "Material",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(7,2)",
                oldPrecision: 7,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Material",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Material",
                table: "Material",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_ServiceRequests_ServiceRequestId",
                table: "Material",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
