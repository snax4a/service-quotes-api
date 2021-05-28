using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceQuotes.Infrastructure.Migrations
{
    public partial class UpdatedQuoteEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServiceRequestId",
                table: "Quotes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_ServiceRequestId",
                table: "Quotes",
                column: "ServiceRequestId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Quotes_ServiceRequests_ServiceRequestId",
                table: "Quotes",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quotes_ServiceRequests_ServiceRequestId",
                table: "Quotes");

            migrationBuilder.DropIndex(
                name: "IX_Quotes_ServiceRequestId",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "ServiceRequestId",
                table: "Quotes");
        }
    }
}
