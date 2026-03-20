namespace CardiTrack.Mobile;

public partial class WelcomePage : ContentPage
{
    public IReadOnlyList<WelcomeSlide> Slides { get; } = WelcomeSlide.DefaultSlides;

    public WelcomePage()
    {
        InitializeComponent();
        SlideCarousel.CurrentItemChanged += (_, _) => UpdateContinueLabel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateContinueLabel();
    }

    private void UpdateContinueLabel()
    {
        if (SlideCarousel.CurrentItem is not WelcomeSlide current)
            return;
        var idx = IndexOf(current);
        ContinueButton.Text = idx >= Slides.Count - 1 ? "Get started" : "Continue";
    }

    private int IndexOf(WelcomeSlide slide)
    {
        for (var i = 0; i < Slides.Count; i++)
        {
            if (ReferenceEquals(Slides[i], slide))
                return i;
        }

        return -1;
    }

    private void OnContinueClicked(object? sender, EventArgs e)
    {
        if (SlideCarousel.CurrentItem is not WelcomeSlide current)
            return;

        var idx = IndexOf(current);
        if (idx < 0 || idx >= Slides.Count - 1)
        {
            WindowNavigation.SetRootPage(this, new AppShell());
            return;
        }

        SlideCarousel.ScrollTo(Slides[idx + 1], position: ScrollToPosition.Center, animate: true);
    }

    private async void OnSignUpTapped(object? sender, EventArgs e)
    {
        await DisplayAlertAsync("Sign up", "Sign-up flow is not wired yet.", "OK");
    }

    private async void OnTermsTapped(object? sender, EventArgs e)
    {
        await DisplayAlertAsync("Terms & Privacy", "Terms and privacy will open here.", "OK");
    }
}
