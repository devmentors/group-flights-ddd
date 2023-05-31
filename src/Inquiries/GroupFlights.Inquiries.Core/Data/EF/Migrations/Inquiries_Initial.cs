using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GroupFlights.Inquiries.Core.Data.EF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "inquiries");

            migrationBuilder.CreateTable(
                name: "Inquiries",
                schema: "inquiries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Inquirer_UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Inquirer_Name = table.Column<string>(type: "text", nullable: true),
                    Inquirer_Surname = table.Column<string>(type: "text", nullable: true),
                    Inquirer_Email = table.Column<string>(type: "text", nullable: true),
                    Inquirer_PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Travel_Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Travel_SourceAirport_Code = table.Column<string>(type: "text", nullable: true),
                    Travel_SourceAirport_City = table.Column<string>(type: "text", nullable: true),
                    Travel_SourceAirport_Name = table.Column<string>(type: "text", nullable: true),
                    Travel_SourceAirport_Country = table.Column<string>(type: "text", nullable: true),
                    Travel_TargetAirport_Code = table.Column<string>(type: "text", nullable: true),
                    Travel_TargetAirport_City = table.Column<string>(type: "text", nullable: true),
                    Travel_TargetAirport_Name = table.Column<string>(type: "text", nullable: true),
                    Travel_TargetAirport_Country = table.Column<string>(type: "text", nullable: true),
                    Return_Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Return_SourceAirport_Code = table.Column<string>(type: "text", nullable: true),
                    Return_SourceAirport_City = table.Column<string>(type: "text", nullable: true),
                    Return_SourceAirport_Name = table.Column<string>(type: "text", nullable: true),
                    Return_SourceAirport_Country = table.Column<string>(type: "text", nullable: true),
                    Return_TargetAirport_Code = table.Column<string>(type: "text", nullable: true),
                    Return_TargetAirport_City = table.Column<string>(type: "text", nullable: true),
                    Return_TargetAirport_Name = table.Column<string>(type: "text", nullable: true),
                    Return_TargetAirport_Country = table.Column<string>(type: "text", nullable: true),
                    DeclaredPassengers_InfantCount = table.Column<int>(type: "integer", nullable: true),
                    DeclaredPassengers_ChildrenCount = table.Column<int>(type: "integer", nullable: true),
                    DeclaredPassengers_AdultCount = table.Column<int>(type: "integer", nullable: true),
                    CheckedBaggageRequired = table.Column<bool>(type: "boolean", nullable: false),
                    AdditionalServicesRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: true),
                    VerificationResult = table.Column<int>(type: "integer", nullable: true),
                    RejectionReason = table.Column<string>(type: "text", nullable: true),
                    OfferId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inquiries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriorityChoice",
                schema: "inquiries",
                columns: table => new
                {
                    InquiryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Feature = table.Column<long>(type: "bigint", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriorityChoice", x => new { x.InquiryId, x.Id });
                    table.ForeignKey(
                        name: "FK_PriorityChoice_Inquiries_InquiryId",
                        column: x => x.InquiryId,
                        principalSchema: "inquiries",
                        principalTable: "Inquiries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriorityChoice",
                schema: "inquiries");

            migrationBuilder.DropTable(
                name: "Inquiries",
                schema: "inquiries");
        }
    }
}
