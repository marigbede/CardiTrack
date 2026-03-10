using CardiTrack.Application.Interfaces.Repositories;
using CardiTrack.Infrastructure.Services;
using CardiTrack.Domain.Entities;
using CardiTrack.Domain.Enums;
using CardiTrack.Infrastructure.ExternalClients;
using CardiTrack.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CardiTrack.UnitTests.Services;

public class FitbitSyncServiceTests
{
    private readonly IOAuthTokenRefreshService _tokenRefresh = Substitute.For<IOAuthTokenRefreshService>();
    private readonly IFitbitApiClient _fitbitApi = Substitute.For<IFitbitApiClient>();
    private readonly IDeviceConnectionRepository _deviceConnections = Substitute.For<IDeviceConnectionRepository>();
    private readonly IActivityLogRepository _activityLogs = Substitute.For<IActivityLogRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly DeviceConnection _fitbitConnection = new()
    {
        Id = Guid.NewGuid(),
        CardiMemberId = Guid.NewGuid(),
        DeviceType = DeviceType.Fitbit,
        ConnectionStatus = ConnectionStatus.Connected,
        IsActive = true
    };

    private readonly DeviceProviderSettings _fitbitConfig = new()
    {
        Provider = "Fitbit",
        ClientId = "test_client",
        ClientSecret = "test_secret",
        TokenUrl = "https://api.fitbit.com/oauth2/token",
        TokenLifetimeHours = 8
    };

    private FitbitSyncService CreateSut()
    {
        var options = Options.Create(new List<DeviceProviderSettings> { _fitbitConfig });
        return new FitbitSyncService(
            _tokenRefresh, _fitbitApi, _deviceConnections, _activityLogs, _unitOfWork, options);
    }

    private void SetupDefaultApiResponses()
    {
        var yesterday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        _fitbitApi.GetActivitiesAsync(Arg.Any<string>(), yesterday)
            .Returns(new FitbitActivitiesResult(8000, 5.2m, 45, 600, 10, 2100));
        _fitbitApi.GetHeartRateAsync(Arg.Any<string>(), yesterday)
            .Returns(new FitbitHeartRateResult(65, 72, 120, 55));
        _fitbitApi.GetSleepAsync(Arg.Any<string>(), yesterday)
            .Returns(new FitbitSleepResult(450, 87, null, null, 90, 240, 90, 30));
    }

    [Fact]
    public async Task SyncCardiMemberAsync_CallsTokenRefresh_BeforeFetchingData()
    {
        _tokenRefresh.RefreshIfExpiredAsync(Arg.Any<DeviceConnection>(), Arg.Any<DeviceProviderSettings>())
            .Returns("access_token");
        SetupDefaultApiResponses();

        await CreateSut().SyncCardiMemberAsync(_fitbitConnection);

        await _tokenRefresh.Received(1).RefreshIfExpiredAsync(_fitbitConnection, _fitbitConfig);
        await _fitbitApi.Received(1).GetActivitiesAsync(Arg.Any<string>(), Arg.Any<DateOnly>());
    }

    [Fact]
    public async Task SyncCardiMemberAsync_MapsApiResponseToActivityLog_Correctly()
    {
        _tokenRefresh.RefreshIfExpiredAsync(Arg.Any<DeviceConnection>(), Arg.Any<DeviceProviderSettings>())
            .Returns("access_token");
        SetupDefaultApiResponses();

        await CreateSut().SyncCardiMemberAsync(_fitbitConnection);

        await _activityLogs.Received(1).UpsertAsync(Arg.Is<ActivityLog>(log =>
            log.Steps == 8000 &&
            log.ActiveMinutes == 45 &&
            log.RestingHeartRate == 65 &&
            log.SleepMinutes == 450 &&
            log.SleepEfficiency == 87));
    }

    [Fact]
    public async Task SyncCardiMemberAsync_UpsertsActivityLog_WithYesterdayDate()
    {
        _tokenRefresh.RefreshIfExpiredAsync(Arg.Any<DeviceConnection>(), Arg.Any<DeviceProviderSettings>())
            .Returns("access_token");
        SetupDefaultApiResponses();

        var expectedDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        await CreateSut().SyncCardiMemberAsync(_fitbitConnection);

        await _activityLogs.Received(1).UpsertAsync(Arg.Is<ActivityLog>(log => log.Date == expectedDate));
    }

    [Fact]
    public async Task SyncCardiMemberAsync_UpdatesLastSyncDate_OnSuccess()
    {
        _tokenRefresh.RefreshIfExpiredAsync(Arg.Any<DeviceConnection>(), Arg.Any<DeviceProviderSettings>())
            .Returns("access_token");
        SetupDefaultApiResponses();

        await CreateSut().SyncCardiMemberAsync(_fitbitConnection);

        await _deviceConnections.Received(1)
            .UpdateLastSyncDateAsync(_fitbitConnection.Id, Arg.Any<DateTime>());
    }

    [Fact]
    public async Task SyncCardiMemberAsync_SetsStatusToSyncError_WhenApiFails()
    {
        _tokenRefresh.RefreshIfExpiredAsync(Arg.Any<DeviceConnection>(), Arg.Any<DeviceProviderSettings>())
            .Returns("access_token");
        var yesterday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        _fitbitApi.GetActivitiesAsync(Arg.Any<string>(), yesterday)
            .ThrowsAsync(new FitbitApiException(500, "Internal Server Error"));

        await Assert.ThrowsAsync<FitbitApiException>(() =>
            CreateSut().SyncCardiMemberAsync(_fitbitConnection));

        await _deviceConnections.Received(1)
            .UpdateStatusAsync(_fitbitConnection.Id, ConnectionStatus.SyncError);
    }

    [Fact]
    public async Task SyncCardiMemberAsync_DoesNotCallApi_WhenTokenRefreshThrows()
    {
        _tokenRefresh.RefreshIfExpiredAsync(Arg.Any<DeviceConnection>(), Arg.Any<DeviceProviderSettings>())
            .ThrowsAsync(new InvalidOperationException("Refresh failed"));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            CreateSut().SyncCardiMemberAsync(_fitbitConnection));

        await _fitbitApi.DidNotReceive().GetActivitiesAsync(Arg.Any<string>(), Arg.Any<DateOnly>());
        await _fitbitApi.DidNotReceive().GetHeartRateAsync(Arg.Any<string>(), Arg.Any<DateOnly>());
        await _fitbitApi.DidNotReceive().GetSleepAsync(Arg.Any<string>(), Arg.Any<DateOnly>());
    }
}
