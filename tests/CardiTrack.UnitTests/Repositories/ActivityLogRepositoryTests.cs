using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Domain.Entities;
using CardiTrack.Domain.Enums;
using CardiTrack.UnitTests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CardiTrack.UnitTests.Repositories;

[Collection("DatabaseCollection")]
public class ActivityLogRepositoryTests(TestDatabaseFixture fixture)
{
    private static ActivityLog BuildLog(Guid cardiMemberId, Guid deviceConnectionId, DateOnly date) =>
        new()
        {
            CardiMemberId = cardiMemberId,
            DeviceConnectionId = deviceConnectionId,
            DataSource = DeviceType.Fitbit,
            Date = date,
            Steps = 8000,
            ActiveMinutes = 45,
            RestingHeartRate = 65,
            SleepMinutes = 420,
            SleepEfficiency = 88
        };

    // ── UpsertAsync ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task UpsertAsync_InsertsNewLog_WhenNoneExists()
    {
        using var scope = fixture.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IActivityLogRepository>();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var org = await TestDataSeeder.SeedOrganizationAsync(scope);
        var member = await TestDataSeeder.SeedCardiMemberAsync(scope, org.Id);
        var connection = await TestDataSeeder.SeedDeviceConnectionAsync(scope, member.Id);

        var date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        var log = BuildLog(member.Id, connection.Id, date);

        await repo.UpsertAsync(log);
        await uow.SaveChangesAsync();

        var results = await repo.GetByCardiMemberAndDateRangeAsync(member.Id, date, date);
        var saved = Assert.Single(results);
        Assert.Equal(8000, saved.Steps);
        Assert.Equal(65, saved.RestingHeartRate);
    }

    [Fact]
    public async Task UpsertAsync_UpdatesExistingLog_WhenMatchFound()
    {
        using var scope = fixture.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IActivityLogRepository>();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var org = await TestDataSeeder.SeedOrganizationAsync(scope);
        var member = await TestDataSeeder.SeedCardiMemberAsync(scope, org.Id);
        var connection = await TestDataSeeder.SeedDeviceConnectionAsync(scope, member.Id);

        var date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2));

        // First upsert — insert
        var initial = BuildLog(member.Id, connection.Id, date);
        await repo.UpsertAsync(initial);
        await uow.SaveChangesAsync();

        // Second upsert — update with different steps
        var updated = BuildLog(member.Id, connection.Id, date);
        updated.Steps = 12000;
        updated.SleepEfficiency = 95;
        await repo.UpsertAsync(updated);
        await uow.SaveChangesAsync();

        var results = await repo.GetByCardiMemberAndDateRangeAsync(member.Id, date, date);
        var saved = Assert.Single(results);
        Assert.Equal(12000, saved.Steps);
        Assert.Equal(95, saved.SleepEfficiency);
    }

    // ── GetByCardiMemberAndDateRangeAsync ────────────────────────────────────────

    [Fact]
    public async Task GetByCardiMemberAndDateRangeAsync_ReturnsLogsWithinRange()
    {
        using var scope = fixture.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IActivityLogRepository>();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var org = await TestDataSeeder.SeedOrganizationAsync(scope);
        var member = await TestDataSeeder.SeedCardiMemberAsync(scope, org.Id);
        var connection = await TestDataSeeder.SeedDeviceConnectionAsync(scope, member.Id);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);
        var twoDaysAgo = today.AddDays(-2);

        await repo.UpsertAsync(BuildLog(member.Id, connection.Id, yesterday));
        await repo.UpsertAsync(BuildLog(member.Id, connection.Id, twoDaysAgo));
        await uow.SaveChangesAsync();

        var results = await repo.GetByCardiMemberAndDateRangeAsync(member.Id, twoDaysAgo, yesterday);

        Assert.Equal(2, results.Count());
        Assert.All(results, log => Assert.Equal(member.Id, log.CardiMemberId));
    }

    [Fact]
    public async Task GetByCardiMemberAndDateRangeAsync_ExcludesLogsOutsideRange()
    {
        using var scope = fixture.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IActivityLogRepository>();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var org = await TestDataSeeder.SeedOrganizationAsync(scope);
        var member = await TestDataSeeder.SeedCardiMemberAsync(scope, org.Id);
        var connection = await TestDataSeeder.SeedDeviceConnectionAsync(scope, member.Id);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var tenDaysAgo = today.AddDays(-10);

        await repo.UpsertAsync(BuildLog(member.Id, connection.Id, tenDaysAgo));
        await uow.SaveChangesAsync();

        // Query last 3 days — ten-days-ago log should be excluded
        var results = await repo.GetByCardiMemberAndDateRangeAsync(
            member.Id, today.AddDays(-3), today);

        Assert.DoesNotContain(results, log => log.Date == tenDaysAgo);
    }

    [Fact]
    public async Task GetByCardiMemberAndDateRangeAsync_ReturnsLogsOrderedByDate()
    {
        using var scope = fixture.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IActivityLogRepository>();
        var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var org = await TestDataSeeder.SeedOrganizationAsync(scope);
        var member = await TestDataSeeder.SeedCardiMemberAsync(scope, org.Id);
        var connection = await TestDataSeeder.SeedDeviceConnectionAsync(scope, member.Id);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // Insert in reverse order
        await repo.UpsertAsync(BuildLog(member.Id, connection.Id, today.AddDays(-1)));
        await repo.UpsertAsync(BuildLog(member.Id, connection.Id, today.AddDays(-3)));
        await repo.UpsertAsync(BuildLog(member.Id, connection.Id, today.AddDays(-2)));
        await uow.SaveChangesAsync();

        var results = (await repo.GetByCardiMemberAndDateRangeAsync(
            member.Id, today.AddDays(-3), today.AddDays(-1))).ToList();

        Assert.Equal(3, results.Count);
        Assert.True(results[0].Date < results[1].Date);
        Assert.True(results[1].Date < results[2].Date);
    }
}
