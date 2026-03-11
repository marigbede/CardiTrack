using System.Net.Http.Headers;
using System.Text.Json;

namespace CardiTrack.Infrastructure.ExternalClients;

public class FitbitApiClient : IFitbitApiClient
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public FitbitApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("FitbitClient");
    }

    public async Task<FitbitActivitiesResult> GetActivitiesAsync(string accessToken, DateOnly date)
    {
        var dateStr = date.ToString("yyyy-MM-dd");
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"/1/user/-/activities/date/{dateStr}.json");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await _httpClient.SendAsync(request);
        await EnsureSuccessAsync(response);

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var summary = doc.RootElement.GetProperty("summary");

        var steps = summary.TryGetProperty("steps", out var s) ? s.GetInt32() : 0;
        var calories = summary.TryGetProperty("caloriesOut", out var c) ? c.GetInt32() : 0;
        var activeMinutes = (summary.TryGetProperty("fairlyActiveMinutes", out var fa) ? fa.GetInt32() : 0)
                          + (summary.TryGetProperty("veryActiveMinutes", out var va) ? va.GetInt32() : 0);
        var sedentaryMinutes = summary.TryGetProperty("sedentaryMinutes", out var sed) ? sed.GetInt32() : 0;
        var floors = summary.TryGetProperty("floors", out var fl) ? fl.GetInt32() : 0;

        decimal distanceKm = 0;
        if (summary.TryGetProperty("distances", out var distances))
        {
            foreach (var d in distances.EnumerateArray())
            {
                if (d.TryGetProperty("activity", out var activity) && activity.GetString() == "total"
                    && d.TryGetProperty("distance", out var dist))
                {
                    distanceKm = dist.GetDecimal();
                    break;
                }
            }
        }

        return new FitbitActivitiesResult(steps, distanceKm, activeMinutes, sedentaryMinutes, floors, calories);
    }

    public async Task<FitbitHeartRateResult> GetHeartRateAsync(string accessToken, DateOnly date)
    {
        var dateStr = date.ToString("yyyy-MM-dd");
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"/1/user/-/heart/date/{dateStr}/1d.json");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await _httpClient.SendAsync(request);
        await EnsureSuccessAsync(response);

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var heartRateDay = doc.RootElement.GetProperty("activities-heart");

        int? restingHr = null;
        int? avgHr = null;
        int? maxHr = null;
        int? minHr = null;

        if (heartRateDay.GetArrayLength() > 0)
        {
            var dayData = heartRateDay[0];
            if (dayData.TryGetProperty("value", out var value))
            {
                if (value.TryGetProperty("restingHeartRate", out var rhr))
                    restingHr = rhr.GetInt32();

                if (value.TryGetProperty("heartRateZones", out var zones))
                {
                    foreach (var zone in zones.EnumerateArray())
                    {
                        if (zone.TryGetProperty("max", out var mx)) maxHr = mx.GetInt32();
                        if (zone.TryGetProperty("min", out var mn) && minHr is null) minHr = mn.GetInt32();
                    }
                }
            }
        }

        return new FitbitHeartRateResult(restingHr, avgHr, maxHr, minHr);
    }

    public async Task<FitbitSleepResult> GetSleepAsync(string accessToken, DateOnly date)
    {
        var dateStr = date.ToString("yyyy-MM-dd");
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"/1/user/-/sleep/date/{dateStr}.json");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var response = await _httpClient.SendAsync(request);
        await EnsureSuccessAsync(response);

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var summary = doc.RootElement.GetProperty("summary");

        var totalMinutes = summary.TryGetProperty("totalMinutesAsleep", out var total) ? total.GetInt32() : 0;
        int? efficiency = null;
        DateTime? startTime = null;
        DateTime? endTime = null;
        int? deep = null, light = null, rem = null, awake = null;

        if (doc.RootElement.TryGetProperty("sleep", out var sleepArr) && sleepArr.GetArrayLength() > 0)
        {
            var mainSleep = sleepArr[0];
            if (mainSleep.TryGetProperty("efficiency", out var eff)) efficiency = eff.GetInt32();
            if (mainSleep.TryGetProperty("startTime", out var st) && DateTime.TryParse(st.GetString(), out var stParsed))
                startTime = stParsed;
            if (mainSleep.TryGetProperty("endTime", out var et) && DateTime.TryParse(et.GetString(), out var etParsed))
                endTime = etParsed;

            if (mainSleep.TryGetProperty("levels", out var levels)
                && levels.TryGetProperty("summary", out var levelSummary))
            {
                if (levelSummary.TryGetProperty("deep", out var deepObj)
                    && deepObj.TryGetProperty("minutes", out var deepMin))
                    deep = deepMin.GetInt32();
                if (levelSummary.TryGetProperty("light", out var lightObj)
                    && lightObj.TryGetProperty("minutes", out var lightMin))
                    light = lightMin.GetInt32();
                if (levelSummary.TryGetProperty("rem", out var remObj)
                    && remObj.TryGetProperty("minutes", out var remMin))
                    rem = remMin.GetInt32();
                if (levelSummary.TryGetProperty("wake", out var wakeObj)
                    && wakeObj.TryGetProperty("minutes", out var wakeMin))
                    awake = wakeMin.GetInt32();
            }
        }

        return new FitbitSleepResult(totalMinutes, efficiency, startTime, endTime, deep, light, rem, awake);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new FitbitApiException((int)response.StatusCode,
                $"Fitbit API returned {(int)response.StatusCode}: {body}");
        }
    }
}
