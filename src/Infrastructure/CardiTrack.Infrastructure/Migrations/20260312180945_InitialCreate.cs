using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CardiTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CardiMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceConnectionId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataSource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Steps = table.Column<int>(type: "integer", nullable: true),
                    Distance = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    ActiveMinutes = table.Column<int>(type: "integer", nullable: true),
                    SedentaryMinutes = table.Column<int>(type: "integer", nullable: true),
                    Floors = table.Column<int>(type: "integer", nullable: true),
                    CaloriesBurned = table.Column<int>(type: "integer", nullable: true),
                    RestingHeartRate = table.Column<int>(type: "integer", nullable: true),
                    AvgHeartRate = table.Column<int>(type: "integer", nullable: true),
                    MaxHeartRate = table.Column<int>(type: "integer", nullable: true),
                    MinHeartRate = table.Column<int>(type: "integer", nullable: true),
                    SleepMinutes = table.Column<int>(type: "integer", nullable: true),
                    SleepStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SleepEndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SleepEfficiency = table.Column<int>(type: "integer", nullable: true),
                    DeepSleepMinutes = table.Column<int>(type: "integer", nullable: true),
                    LightSleepMinutes = table.Column<int>(type: "integer", nullable: true),
                    RemSleepMinutes = table.Column<int>(type: "integer", nullable: true),
                    AwakeMinutes = table.Column<int>(type: "integer", nullable: true),
                    SpO2Average = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    SpO2Min = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    SpO2Max = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    VO2Max = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    StressScore = table.Column<int>(type: "integer", nullable: true),
                    BreathingRate = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    Temperature = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CardiMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    AlertType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Severity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    TriggeredDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    AcknowledgedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AcknowledgedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsResolved = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MetricValues = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CardiMemberId = table.Column<Guid>(type: "uuid", nullable: true),
                    Action = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    RequestPath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    HttpMethod = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ResponseStatus = table.Column<int>(type: "integer", nullable: false),
                    DataAccessed = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ChangedFields = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardiMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EmergencyContactName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    MedicalNotes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardiMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceConnections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CardiMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DeviceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ConnectionStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AccessToken = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    RefreshToken = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    TokenExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Scopes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, defaultValue: "[]"),
                    ConnectedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SyncFrequencyMinutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 30),
                    Metadata = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceConnections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeviceType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Manufacturer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ModelName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Capabilities = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false, defaultValue: "{}"),
                    ApiEndpoint = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OAuthConfig = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IconUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatternBaselines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CardiMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    CalculatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    PeriodDays = table.Column<int>(type: "integer", nullable: false),
                    AvgSteps = table.Column<int>(type: "integer", nullable: true),
                    StdDevSteps = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    AvgActiveMinutes = table.Column<int>(type: "integer", nullable: true),
                    AvgRestingHeartRate = table.Column<int>(type: "integer", nullable: true),
                    StdDevHeartRate = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    MaxHeartRateObserved = table.Column<int>(type: "integer", nullable: true),
                    AvgSleepMinutes = table.Column<int>(type: "integer", nullable: true),
                    TypicalBedtime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    TypicalWakeTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    AvgSleepEfficiency = table.Column<int>(type: "integer", nullable: true),
                    StepsByDayOfWeek = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatternBaselines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Tier = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TrialEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BillingCycle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "USD"),
                    MaxCardiMembers = table.Column<int>(type: "integer", nullable: false),
                    MaxUsers = table.Column<int>(type: "integer", nullable: false),
                    Features = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false, defaultValue: "{}"),
                    PaymentMethod = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Auth0UserId = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCardiMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CardiMemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    RelationshipType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsPrimaryCaregiver = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanViewHealthData = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ReceiveAlerts = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    NotificationPreferences = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false, defaultValue: "{}"),
                    AssignedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCardiMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCardiMembers_CardiMembers_CardiMemberId",
                        column: x => x.CardiMemberId,
                        principalTable: "CardiMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCardiMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "ApiEndpoint", "Capabilities", "CreatedDate", "DeviceType", "DisplayName", "IconUrl", "IsActive", "Manufacturer", "ModelName", "OAuthConfig", "SortOrder", "UpdatedDate" },
                values: new object[] { new Guid("a1b2c3d4-0001-0000-0000-000000000001"), "https://api.fitbit.com", "{\"hasHeartRate\":true,\"hasSleep\":true,\"hasActivity\":true,\"hasSpO2\":true,\"hasStress\":false}", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fitbit", "Fitbit", null, true, "Fitbit / Google", "All Models", null, 1, null });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "ApiEndpoint", "Capabilities", "CreatedDate", "DeviceType", "DisplayName", "IconUrl", "Manufacturer", "ModelName", "OAuthConfig", "SortOrder", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-0002-0000-0000-000000000002"), null, "{\"hasHeartRate\":true,\"hasSleep\":true,\"hasActivity\":true,\"hasSpO2\":true,\"hasECG\":true}", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "AppleWatch", "Apple Watch", null, "Apple", "All Models", null, 2, null },
                    { new Guid("a1b2c3d4-0003-0000-0000-000000000003"), "https://healthapi.garmin.com", "{\"hasHeartRate\":true,\"hasSleep\":true,\"hasActivity\":true,\"hasSpO2\":true,\"hasStress\":true}", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Garmin", "Garmin", null, "Garmin", "All Models", null, 3, null },
                    { new Guid("a1b2c3d4-0004-0000-0000-000000000004"), "https://api.shealth.samsung.com", "{\"hasHeartRate\":true,\"hasSleep\":true,\"hasActivity\":true,\"hasSpO2\":true}", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Samsung", "Samsung Galaxy Watch", null, "Samsung", "Galaxy Watch", null, 4, null },
                    { new Guid("a1b2c3d4-0005-0000-0000-000000000005"), "https://wbsapi.withings.net", "{\"hasHeartRate\":true,\"hasSleep\":true,\"hasActivity\":true,\"hasSpO2\":true,\"hasTemperature\":true}", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Withings", "Withings", null, "Withings", "All Models", null, 5, null },
                    { new Guid("a1b2c3d4-0006-0000-0000-000000000006"), "https://api.ouraring.com", "{\"hasHeartRate\":true,\"hasSleep\":true,\"hasActivity\":true,\"hasSpO2\":true,\"hasTemperature\":true,\"hasHRV\":true}", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Oura", "Oura Ring", null, "Oura", "Oura Ring", null, 6, null },
                    { new Guid("a1b2c3d4-0007-0000-0000-000000000007"), "https://api.prod.whoop.com", "{\"hasHeartRate\":true,\"hasSleep\":true,\"hasActivity\":true,\"hasHRV\":true,\"hasStress\":true}", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Whoop", "WHOOP", null, "WHOOP", "WHOOP Strap", null, 7, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_CardiMemberId",
                table: "ActivityLogs",
                column: "CardiMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_CardiMemberId_Date",
                table: "ActivityLogs",
                columns: new[] { "CardiMemberId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_DataSource",
                table: "ActivityLogs",
                column: "DataSource");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_Date",
                table: "ActivityLogs",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_DeviceConnectionId",
                table: "ActivityLogs",
                column: "DeviceConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_AlertType",
                table: "Alerts",
                column: "AlertType");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_CardiMemberId",
                table: "Alerts",
                column: "CardiMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_CardiMemberId_IsResolved_TriggeredDate",
                table: "Alerts",
                columns: new[] { "CardiMemberId", "IsResolved", "TriggeredDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_IsResolved",
                table: "Alerts",
                column: "IsResolved");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_Severity",
                table: "Alerts",
                column: "Severity");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_TriggeredDate",
                table: "Alerts",
                column: "TriggeredDate");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CardiMemberId",
                table: "AuditLogs",
                column: "CardiMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CardiMemberId_Timestamp",
                table: "AuditLogs",
                columns: new[] { "CardiMemberId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityType",
                table: "AuditLogs",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Timestamp",
                table: "AuditLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId_Timestamp",
                table: "AuditLogs",
                columns: new[] { "UserId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_CardiMembers_IsActive",
                table: "CardiMembers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CardiMembers_LastSyncDate",
                table: "CardiMembers",
                column: "LastSyncDate");

            migrationBuilder.CreateIndex(
                name: "IX_CardiMembers_OrganizationId",
                table: "CardiMembers",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConnections_CardiMemberId",
                table: "DeviceConnections",
                column: "CardiMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConnections_CardiMemberId_IsPrimary",
                table: "DeviceConnections",
                columns: new[] { "CardiMemberId", "IsPrimary" });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConnections_ConnectionStatus",
                table: "DeviceConnections",
                column: "ConnectionStatus");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConnections_DeviceType",
                table: "DeviceConnections",
                column: "DeviceType");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConnections_LastSyncDate",
                table: "DeviceConnections",
                column: "LastSyncDate");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceType",
                table: "Devices",
                column: "DeviceType");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_IsActive",
                table: "Devices",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_SortOrder",
                table: "Devices",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_IsActive",
                table: "Organizations",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Type",
                table: "Organizations",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_PatternBaselines_CalculatedDate",
                table: "PatternBaselines",
                column: "CalculatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_PatternBaselines_CardiMemberId",
                table: "PatternBaselines",
                column: "CardiMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PatternBaselines_CardiMemberId_PeriodDays_CalculatedDate",
                table: "PatternBaselines",
                columns: new[] { "CardiMemberId", "PeriodDays", "CalculatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_OrganizationId",
                table: "Subscriptions",
                column: "OrganizationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Status",
                table: "Subscriptions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Status_EndDate",
                table: "Subscriptions",
                columns: new[] { "Status", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_UserCardiMembers_CardiMemberId",
                table: "UserCardiMembers",
                column: "CardiMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCardiMembers_IsPrimaryCaregiver",
                table: "UserCardiMembers",
                column: "IsPrimaryCaregiver");

            migrationBuilder.CreateIndex(
                name: "IX_UserCardiMembers_UserId",
                table: "UserCardiMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCardiMembers_UserId_CardiMemberId_IsActive",
                table: "UserCardiMembers",
                columns: new[] { "UserId", "CardiMemberId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsActive",
                table: "Users",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationId",
                table: "Users",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "DeviceConnections");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "PatternBaselines");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "UserCardiMembers");

            migrationBuilder.DropTable(
                name: "CardiMembers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
