using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupFlights.WorkloadManagement.Core.Data.EF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "workloads");

            migrationBuilder.CreateTable(
                name: "Workloads",
                schema: "workloads",
                columns: table => new
                {
                    AssignmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CashierId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkloadType = table.Column<string>(type: "text", nullable: false),
                    WorkloadSourceId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workloads", x => x.AssignmentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workloads_CashierId_WorkloadType_WorkloadSourceId",
                schema: "workloads",
                table: "Workloads",
                columns: new[] { "CashierId", "WorkloadType", "WorkloadSourceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workloads_WorkloadType_WorkloadSourceId",
                schema: "workloads",
                table: "Workloads",
                columns: new[] { "WorkloadType", "WorkloadSourceId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workloads",
                schema: "workloads");
        }
    }
}
