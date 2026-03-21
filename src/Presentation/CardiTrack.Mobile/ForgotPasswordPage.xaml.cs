namespace CardiTrack.Mobile;

public partial class ForgotPasswordPage : ContentPage
{
    private readonly Entry[] _codeEntries;

    public ForgotPasswordPage()
    {
        InitializeComponent();
        _codeEntries = [Code0, Code1, Code2, Code3, Code4, Code5];
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Code0.Focus();
    }

    private void OnCodeDigitChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not Entry current || string.IsNullOrEmpty(e.NewTextValue))
            return;

        var idx = Array.IndexOf(_codeEntries, current);
        if (idx < 0 || idx >= _codeEntries.Length - 1)
            return;

        _codeEntries[idx + 1].Focus();
    }

    private async void OnVerifyClicked(object? sender, EventArgs e)
    {
        var code = string.Concat(_codeEntries.Select(c => c.Text ?? ""));
        if (code.Length < 6)
        {
            await DisplayAlertAsync("Incomplete", "Please enter all 6 digits.", "OK");
            return;
        }

        VerifyBtn.Text = "Verifying...";
        VerifyBtn.IsEnabled = false;

        try
        {
            await Task.Delay(1200);
            await DisplayAlertAsync("Success", "Code verified (placeholder).", "OK");
            await Navigation.PopToRootAsync();
        }
        catch
        {
            await DisplayAlertAsync("Error", "Verification failed. Please try again.", "OK");
        }
        finally
        {
            VerifyBtn.Text = "Verify Code";
            VerifyBtn.IsEnabled = true;
        }
    }

    private void OnEntryFocused(object? sender, FocusEventArgs e)
    {
        if (sender is Entry entry && entry.Parent is Border border)
            border.Stroke = new SolidColorBrush((Color)Microsoft.Maui.Controls.Application.Current!.Resources["InputFocusBorder"]);
    }

    private void OnEntryUnfocused(object? sender, FocusEventArgs e)
    {
        if (sender is Entry entry && entry.Parent is Border border)
            border.Stroke = new SolidColorBrush((Color)Microsoft.Maui.Controls.Application.Current!.Resources["InputBorder"]);
    }

    private async void OnResendTapped(object? sender, EventArgs e)
    {
        await DisplayAlertAsync("Code Sent", "A new code has been sent to your email.", "OK");
    }
}