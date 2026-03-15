using System.Net;
using CardiTrack.Infrastructure.ExternalClients;
using NSubstitute;

namespace CardiTrack.UnitTests.ExternalClients;

public class FitbitApiClientTests
{
    private static IFitbitApiClient CreateSut(string responseBody, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var handler = new FakeHttpHandler(responseBody, statusCode);
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.fitbit.com") };
        var factory = Substitute.For<IHttpClientFactory>();
        factory.CreateClient("FitbitClient").Returns(httpClient);
        return new FitbitApiClient(factory);
    }

    private static DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);

    // ── Activities ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetActivitiesAsync_ReturnsSteps_FromApiResponse()
    {
        var json = """
            {
              "summary": {
                "steps": 9423,
                "caloriesOut": 2200,
                "fairlyActiveMinutes": 20,
                "veryActiveMinutes": 25,
                "sedentaryMinutes": 700,
                "floors": 8,
                "distances": [{ "activity": "total", "distance": 6.3 }]
              }
            }
            """;

        var result = await CreateSut(json).GetActivitiesAsync("token", Today);

        Assert.Equal(9423, result.Steps);
    }

    [Fact]
    public async Task GetActivitiesAsync_ReturnsActiveMinutes_FromApiResponse()
    {
        var json = """
            {
              "summary": {
                "steps": 0,
                "caloriesOut": 0,
                "fairlyActiveMinutes": 15,
                "veryActiveMinutes": 30,
                "sedentaryMinutes": 800,
                "floors": 0,
                "distances": []
              }
            }
            """;

        var result = await CreateSut(json).GetActivitiesAsync("token", Today);

        Assert.Equal(45, result.ActiveMinutes); // 15 fairly + 30 very
    }

    [Fact]
    public async Task GetActivitiesAsync_ThrowsFitbitApiException_OnNon2xxResponse()
    {
        await Assert.ThrowsAsync<FitbitApiException>(() =>
            CreateSut("{\"errors\":[{\"errorType\":\"expired_token\"}]}", HttpStatusCode.Unauthorized)
                .GetActivitiesAsync("bad_token", Today));
    }

    // ── Heart Rate ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetHeartRateAsync_ReturnsRestingHeartRate_FromApiResponse()
    {
        var json = """
            {
              "activities-heart": [
                {
                  "value": {
                    "restingHeartRate": 63,
                    "heartRateZones": [
                      { "name": "Out of Range", "min": 30, "max": 93 },
                      { "name": "Fat Burn",     "min": 93, "max": 130 },
                      { "name": "Cardio",       "min": 130, "max": 157 },
                      { "name": "Peak",         "min": 157, "max": 220 }
                    ]
                  }
                }
              ]
            }
            """;

        var result = await CreateSut(json).GetHeartRateAsync("token", Today);

        Assert.Equal(63, result.RestingHeartRate);
    }

    [Fact]
    public async Task GetHeartRateAsync_ThrowsFitbitApiException_OnNon2xxResponse()
    {
        await Assert.ThrowsAsync<FitbitApiException>(() =>
            CreateSut("{}", HttpStatusCode.InternalServerError)
                .GetHeartRateAsync("token", Today));
    }

    // ── Sleep ────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetSleepAsync_ReturnsTotalSleepMinutes_FromApiResponse()
    {
        var json = """
            {
              "summary": { "totalMinutesAsleep": 426 },
              "sleep": [
                {
                  "efficiency": 91,
                  "startTime": "2026-03-09T22:30:00.000",
                  "endTime":   "2026-03-10T06:30:00.000",
                  "levels": {
                    "summary": {
                      "deep":  { "minutes": 85 },
                      "light": { "minutes": 220 },
                      "rem":   { "minutes": 90 },
                      "wake":  { "minutes": 25 }
                    }
                  }
                }
              ]
            }
            """;

        var result = await CreateSut(json).GetSleepAsync("token", Today);

        Assert.Equal(426, result.TotalSleepMinutes);
    }

    [Fact]
    public async Task GetSleepAsync_ReturnsSleepStages_FromApiResponse()
    {
        var json = """
            {
              "summary": { "totalMinutesAsleep": 426 },
              "sleep": [
                {
                  "efficiency": 91,
                  "startTime": "2026-03-09T22:30:00.000",
                  "endTime":   "2026-03-10T06:30:00.000",
                  "levels": {
                    "summary": {
                      "deep":  { "minutes": 85 },
                      "light": { "minutes": 220 },
                      "rem":   { "minutes": 90 },
                      "wake":  { "minutes": 25 }
                    }
                  }
                }
              ]
            }
            """;

        var result = await CreateSut(json).GetSleepAsync("token", Today);

        Assert.Equal(85, result.DeepSleepMinutes);
        Assert.Equal(220, result.LightSleepMinutes);
        Assert.Equal(90, result.RemSleepMinutes);
        Assert.Equal(25, result.AwakeMinutes);
        Assert.Equal(91, result.SleepEfficiency);
    }

    [Fact]
    public async Task GetSleepAsync_ThrowsFitbitApiException_OnNon2xxResponse()
    {
        await Assert.ThrowsAsync<FitbitApiException>(() =>
            CreateSut("{}", HttpStatusCode.Unauthorized)
                .GetSleepAsync("bad_token", Today));
    }
}
