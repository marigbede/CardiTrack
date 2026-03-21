namespace CardiTrack.Mobile;

public partial class SplashPage : ContentPage
{
    private const bool SimulateStartupFailure = false;

    private bool _scheduledInitialStartup;

    public SplashPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_scheduledInitialStartup)
            return;
        _scheduledInitialStartup = true;
        _ = RunStartupAsync();
    }

    private async void OnRetryClicked(object? sender, EventArgs e)
    {
        LoadingPanel.IsVisible = true;
        ErrorPanel.IsVisible = false;
        await RunStartupAsync();
    }

    private async Task RunStartupAsync()
    {
        try
        {
            await Task.Delay(900);

#if DEBUG
            if (SimulateStartupFailure)
                throw new InvalidOperationException("Simulated splash failure.");
#endif

            await Task.Delay(200);

            await MainThread.InvokeOnMainThreadAsync(() =>
                WindowNavigation.SetRootPage(this, new WelcomePage()));
        }
        catch
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                LoadingPanel.IsVisible = false;
                ErrorPanel.IsVisible = true;
            });
        }
    }
}