namespace CardiTrack.Mobile;

internal static class WindowNavigation
{
    public static void SetRootPage(Page current, Page newRoot)
    {
        var window = current.Window
            ?? global::Microsoft.Maui.Controls.Application.Current?.Windows.FirstOrDefault();
        if (window is null)
            return;
        window.Page = newRoot;
    }
}
