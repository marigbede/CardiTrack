namespace CardiTrack.Infrastructure.ExternalClients;

public class FitbitApiException : Exception
{
    public int StatusCode { get; }

    public FitbitApiException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}
