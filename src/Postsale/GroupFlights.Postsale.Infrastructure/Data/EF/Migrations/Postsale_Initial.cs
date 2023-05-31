using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GroupFlights.Postsale.Infrastructure.Data.EF.Migrations
{
    public partial class Initial_Postsale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "postsale");

            migrationBuilder.CreateTable(
                name: "ChangesToApply",
                schema: "postsale",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    _passengerNamesDeadlineChange_DeadlineId = table.Column<Guid>(type: "uuid", nullable: true),
                    _passengerNamesDeadlineChange_NewDueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    _costAfterChange_TotalCost_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    _costAfterChange_TotalCost_Currency = table.Column<int>(type: "integer", nullable: false),
                    _costAfterChange_RefundableCost_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    _costAfterChange_RefundableCost_Currency = table.Column<int>(type: "integer", nullable: false),
                    _costAfterChange_Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangesToApply", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReservationSnapshots",
                schema: "postsale",
                columns: table => new
                {
                    ReservationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AirlineType = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CurrentCost_TotalCost_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrentCost_TotalCost_Currency = table.Column<int>(type: "integer", nullable: false),
                    CurrentCost_RefundableCost_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrentCost_RefundableCost_Currency = table.Column<int>(type: "integer", nullable: false),
                    CurrentCost_Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PassengerNamesDeadline_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    PassengerNamesDeadline_DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PassengerNamesDeadline_Fulfilled = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationSnapshots", x => x.ReservationId);
                });

            migrationBuilder.CreateTable(
                name: "ChangesToApply_NewTravelSegments",
                schema: "postsale",
                columns: table => new
                {
                    TravelChangeReservationChangeToApplyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SourceAirport_Code = table.Column<string>(type: "text", nullable: true),
                    SourceAirport_City = table.Column<string>(type: "text", nullable: true),
                    SourceAirport_Name = table.Column<string>(type: "text", nullable: true),
                    SourceAirport_Country = table.Column<string>(type: "text", nullable: true),
                    TargetAirport_Code = table.Column<string>(type: "text", nullable: true),
                    TargetAirport_City = table.Column<string>(type: "text", nullable: true),
                    TargetAirport_Name = table.Column<string>(type: "text", nullable: true),
                    TargetAirport_Country = table.Column<string>(type: "text", nullable: true),
                    FlightTime_Hours = table.Column<int>(type: "integer", nullable: true),
                    FlightTime_Minutes = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangesToApply_NewTravelSegments", x => new { x.TravelChangeReservationChangeToApplyId, x.Id });
                    table.ForeignKey(
                        name: "FK_ChangesToApply_NewTravelSegments_ChangesToApply_TravelChang~",
                        column: x => x.TravelChangeReservationChangeToApplyId,
                        principalSchema: "postsale",
                        principalTable: "ChangesToApply",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDeadlineChange",
                schema: "postsale",
                columns: table => new
                {
                    ReservationChangeToApplyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PaymentId = table.Column<Guid>(type: "uuid", nullable: false),
                    NewDueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeadlineId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDeadlineChange", x => new { x.ReservationChangeToApplyId, x.Id });
                    table.ForeignKey(
                        name: "FK_PaymentDeadlineChange_ChangesToApply_ReservationChangeToApp~",
                        column: x => x.ReservationChangeToApplyId,
                        principalSchema: "postsale",
                        principalTable: "ChangesToApply",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequests",
                schema: "postsale",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    _changeToApplyId = table.Column<Guid>(type: "uuid", nullable: true),
                    _completionStatus = table.Column<int>(type: "integer", nullable: true),
                    _isActive = table.Column<bool>(type: "boolean", nullable: false),
                    _isFeasible = table.Column<bool>(type: "boolean", nullable: false),
                    _newTravelDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    _requester = table.Column<Guid>(type: "uuid", nullable: false),
                    _reservationToChangeReservationId = table.Column<Guid>(type: "uuid", nullable: true),
                    _newCost_TotalCost_Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    _newCost_TotalCost_Currency = table.Column<int>(type: "integer", nullable: true),
                    _newCost_RefundableCost_Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    _newCost_RefundableCost_Currency = table.Column<int>(type: "integer", nullable: true),
                    _newCost_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    _paymentRequiredToApplyChange_PaymentId = table.Column<Guid>(type: "uuid", nullable: true),
                    _paymentRequiredToApplyChange_Deadline_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    _paymentRequiredToApplyChange_Deadline_DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    _paymentRequiredToApplyChange_Deadline_Fulfilled = table.Column<bool>(type: "boolean", nullable: true),
                    _paymentRequiredToApplyChange_Payed = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeRequests_ChangesToApply__changeToApplyId",
                        column: x => x._changeToApplyId,
                        principalSchema: "postsale",
                        principalTable: "ChangesToApply",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChangeRequests_ReservationSnapshots__reservationToChangeRes~",
                        column: x => x._reservationToChangeReservationId,
                        principalSchema: "postsale",
                        principalTable: "ReservationSnapshots",
                        principalColumn: "ReservationId");
                });

            migrationBuilder.CreateTable(
                name: "ReservationSnapshots_CurrentPayments",
                schema: "postsale",
                columns: table => new
                {
                    ReservationToChangeReservationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PaymentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Deadline_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    Deadline_DueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Deadline_Fulfilled = table.Column<bool>(type: "boolean", nullable: true),
                    Payed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationSnapshots_CurrentPayments", x => new { x.ReservationToChangeReservationId, x.Id });
                    table.ForeignKey(
                        name: "FK_ReservationSnapshots_CurrentPayments_ReservationSnapshots_R~",
                        column: x => x.ReservationToChangeReservationId,
                        principalSchema: "postsale",
                        principalTable: "ReservationSnapshots",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationSnapshots_CurrentTravel",
                schema: "postsale",
                columns: table => new
                {
                    ReservationToChangeReservationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SourceAirport_Code = table.Column<string>(type: "text", nullable: true),
                    SourceAirport_City = table.Column<string>(type: "text", nullable: true),
                    SourceAirport_Name = table.Column<string>(type: "text", nullable: true),
                    SourceAirport_Country = table.Column<string>(type: "text", nullable: true),
                    TargetAirport_Code = table.Column<string>(type: "text", nullable: true),
                    TargetAirport_City = table.Column<string>(type: "text", nullable: true),
                    TargetAirport_Name = table.Column<string>(type: "text", nullable: true),
                    TargetAirport_Country = table.Column<string>(type: "text", nullable: true),
                    FlightTime_Hours = table.Column<int>(type: "integer", nullable: true),
                    FlightTime_Minutes = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationSnapshots_CurrentTravel", x => new { x.ReservationToChangeReservationId, x.Id });
                    table.ForeignKey(
                        name: "FK_ReservationSnapshots_CurrentTravel_ReservationSnapshots_Res~",
                        column: x => x.ReservationToChangeReservationId,
                        principalSchema: "postsale",
                        principalTable: "ReservationSnapshots",
                        principalColumn: "ReservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChangeRequests__newTravel",
                schema: "postsale",
                columns: table => new
                {
                    ReservationChangeRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SourceAirport_Code = table.Column<string>(type: "text", nullable: true),
                    SourceAirport_City = table.Column<string>(type: "text", nullable: true),
                    SourceAirport_Name = table.Column<string>(type: "text", nullable: true),
                    SourceAirport_Country = table.Column<string>(type: "text", nullable: true),
                    TargetAirport_Code = table.Column<string>(type: "text", nullable: true),
                    TargetAirport_City = table.Column<string>(type: "text", nullable: true),
                    TargetAirport_Name = table.Column<string>(type: "text", nullable: true),
                    TargetAirport_Country = table.Column<string>(type: "text", nullable: true),
                    FlightTime_Hours = table.Column<int>(type: "integer", nullable: true),
                    FlightTime_Minutes = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeRequests__newTravel", x => new { x.ReservationChangeRequestId, x.Id });
                    table.ForeignKey(
                        name: "FK_ChangeRequests__newTravel_ChangeRequests_ReservationChangeR~",
                        column: x => x.ReservationChangeRequestId,
                        principalSchema: "postsale",
                        principalTable: "ChangeRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequests__changeToApplyId",
                schema: "postsale",
                table: "ChangeRequests",
                column: "_changeToApplyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeRequests__reservationToChangeReservationId",
                schema: "postsale",
                table: "ChangeRequests",
                column: "_reservationToChangeReservationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeRequests__newTravel",
                schema: "postsale");

            migrationBuilder.DropTable(
                name: "ChangesToApply_NewTravelSegments",
                schema: "postsale");

            migrationBuilder.DropTable(
                name: "PaymentDeadlineChange",
                schema: "postsale");

            migrationBuilder.DropTable(
                name: "ReservationSnapshots_CurrentPayments",
                schema: "postsale");

            migrationBuilder.DropTable(
                name: "ReservationSnapshots_CurrentTravel",
                schema: "postsale");

            migrationBuilder.DropTable(
                name: "ChangeRequests",
                schema: "postsale");

            migrationBuilder.DropTable(
                name: "ChangesToApply",
                schema: "postsale");

            migrationBuilder.DropTable(
                name: "ReservationSnapshots",
                schema: "postsale");
        }
    }
}
