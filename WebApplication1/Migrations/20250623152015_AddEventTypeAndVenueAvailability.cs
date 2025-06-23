using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTypeAndVenueAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventTypeId",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    EventTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.EventTypeId);
                });

            migrationBuilder.CreateTable(
                name: "VenueAvailabilityPeriods",
                columns: table => new
                {
                    VenueAvailabilityPeriodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VenueId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueAvailabilityPeriods", x => x.VenueAvailabilityPeriodId);
                    table.ForeignKey(
                        name: "FK_VenueAvailabilityPeriods_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "VenueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventTypeId",
                table: "Events",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VenueAvailabilityPeriods_VenueId",
                table: "VenueAvailabilityPeriods",
                column: "VenueId");

            // Seed EventTypes
            migrationBuilder.InsertData(
                table: "EventTypes",
                columns: new[] { "Name", "Description", "IsActive" },
                values: new object[,]
                {
                    { "Conference", "Professional gatherings, seminars, and business meetings", true },
                    { "Wedding", "Wedding ceremonies and receptions", true },
                    { "Concert", "Musical performances and live entertainment events", true },
                    { "Exhibition", "Art exhibitions, trade shows, and product launches", true },
                    { "Corporate Event", "Team building, workshops, and company celebrations", true },
                    { "Social Gathering", "Birthday parties, anniversaries, and social celebrations", true },
                    { "Sports Event", "Sports competitions and athletic events", true },
                    { "Workshop", "Educational and training sessions", true },
                    { "Charity Event", "Fundraising and charitable gatherings", true },
                    { "Private Party", "Private celebrations and exclusive gatherings", true }
                });

            // Set default EventTypeId for existing events
            migrationBuilder.Sql("UPDATE Events SET EventTypeId = (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Private Party')");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventTypes_EventTypeId",
                table: "Events",
                column: "EventTypeId",
                principalTable: "EventTypes",
                principalColumn: "EventTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventTypes_EventTypeId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropTable(
                name: "VenueAvailabilityPeriods");

            migrationBuilder.DropIndex(
                name: "IX_Events_EventTypeId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventTypeId",
                table: "Events");
        }
    }
}
