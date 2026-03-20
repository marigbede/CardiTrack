namespace CardiTrack.Mobile;

public sealed class WelcomeSlide
{
    public required ImageSource HeroImage { get; init; }
    public required string Title { get; init; }
    public required string Subtitle { get; init; }

    public static IReadOnlyList<WelcomeSlide> DefaultSlides { get; } =
    [
        new WelcomeSlide
        {
            HeroImage = ImageSource.FromFile("welcome_hero_a.svg"),
            Title = "Know They're Okay",
            Subtitle = "Stay close to the people you love — even from far away",
        },
        new WelcomeSlide
        {
            HeroImage = ImageSource.FromFile("welcome_hero_b.svg"),
            Title = "Their Watch, Your Peace of Mind",
            Subtitle = "Connects with Fitbit, Apple Watch, Garmin & more",
        },
        new WelcomeSlide
        {
            HeroImage = ImageSource.FromFile("welcome_hero_c.svg"),
            Title = "Care Together",
            Subtitle = "Share the watch with your siblings — you're not in this alone",
        },
    ];
}
