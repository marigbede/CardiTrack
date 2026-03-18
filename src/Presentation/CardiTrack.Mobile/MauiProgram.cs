using Microsoft.Extensions.Logging;

namespace CardiTrack.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // HTTP CLIENT FACTORY — named client targeting the CardiTrack API
        builder.Services.AddHttpClient("CardiTrackApiClient", client =>
        {
            client.BaseAddress = new Uri(ApiConstants.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
