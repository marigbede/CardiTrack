namespace CardiTrack.Infrastructure.ExternalClients;

public interface IFitbitApiClient
{
    Task<FitbitActivitiesResult> GetActivitiesAsync(string accessToken, DateOnly date);
    Task<FitbitHeartRateResult> GetHeartRateAsync(string accessToken, DateOnly date);
    Task<FitbitSleepResult> GetSleepAsync(string accessToken, DateOnly date);
}
