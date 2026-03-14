using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardiTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLocale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Locale",
                table: "Users",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "en-US");

            migrationBuilder.AddColumn<string>(
                name: "TimeZoneId",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "UTC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Locale",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TimeZoneId",
                table: "Users");
        }
    }
}
