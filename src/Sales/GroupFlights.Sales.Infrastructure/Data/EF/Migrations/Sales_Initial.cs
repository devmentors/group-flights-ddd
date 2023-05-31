using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupFlights.Sales.Infrastructure.Data.EF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sales");

            migrationBuilder.CreateTable(
                name: "DeadlineRegistryEntries",
                schema: "sales",
                columns: table => new
                {
                    DeadlineId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlineRegistryEntries", x => x.DeadlineId);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                schema: "sales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Object = table.Column<string>(type: "jsonb", nullable: false),
                    Version = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                schema: "sales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Object = table.Column<string>(type: "jsonb", nullable: false),
                    ContractId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdateDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeadlineRegistryEntries",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "Offers",
                schema: "sales");

            migrationBuilder.DropTable(
                name: "Reservations",
                schema: "sales");
        }
    }
}
