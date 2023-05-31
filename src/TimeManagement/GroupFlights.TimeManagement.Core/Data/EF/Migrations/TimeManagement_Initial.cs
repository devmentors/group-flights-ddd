using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroupFlights.TimeManagement.Core.Data.EF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "time-management");

            migrationBuilder.CreateTable(
                name: "Deadlines",
                schema: "time-management",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CommunicationChannel = table.Column<int>(type: "integer", nullable: false),
                    Message_Content = table.Column<string>(type: "text", nullable: true),
                    Message_MessageTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeadlineDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Fulfilled = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deadlines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeadlineParticipant",
                schema: "time-management",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    DeadlineId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeadlineParticipant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeadlineParticipant_Deadlines_DeadlineId",
                        column: x => x.DeadlineId,
                        principalSchema: "time-management",
                        principalTable: "Deadlines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "time-management",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    DeadlineId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Deadlines_DeadlineId",
                        column: x => x.DeadlineId,
                        principalSchema: "time-management",
                        principalTable: "Deadlines",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeadlineParticipant_DeadlineId",
                schema: "time-management",
                table: "DeadlineParticipant",
                column: "DeadlineId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_DeadlineId",
                schema: "time-management",
                table: "Notifications",
                column: "DeadlineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeadlineParticipant",
                schema: "time-management");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "time-management");

            migrationBuilder.DropTable(
                name: "Deadlines",
                schema: "time-management");
        }
    }
}
