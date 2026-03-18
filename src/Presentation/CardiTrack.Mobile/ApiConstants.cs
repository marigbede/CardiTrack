namespace CardiTrack.Mobile;

public static class ApiConstants
{
#if DEBUG
#if ANDROID
    // Android emulator routes 10.0.2.2 to the host machine's localhost
    public const string BaseUrl = "https://10.0.2.2:7001";
#else
    // iOS simulator and Windows dev host use localhost directly
    public const string BaseUrl = "https://localhost:7001";
#endif
#else
    public const string BaseUrl = "https://api.carditrack.com";
#endif
}
