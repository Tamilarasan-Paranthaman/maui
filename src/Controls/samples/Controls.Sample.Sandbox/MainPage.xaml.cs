using System.ComponentModel;
using System.Diagnostics;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
    private const uint AnimationDuration = 250;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnFooterPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        Debug.WriteLine($"FooterView PropertyChanged: {e.PropertyName}");
    }

    private void OnFooterButtonClicked(object sender, EventArgs e)
    {
        Debug.WriteLine("OnFooterButtonClicked");
        if (!FooterView.IsVisible)
            Dispatcher.DispatchAsync(ShowFooter);
        else
            Dispatcher.DispatchAsync(HideFooter);
    }

    private async Task ShowFooter()
    {
        if (FooterView.IsVisible)
            return;

        Debug.WriteLine("Showing Footer");

        var height = FooterView.Measure(FooterView.Width, double.PositiveInfinity).Height;
        FooterView.TranslationY = height;
        FooterView.IsVisible = true;

        // This causes deadlock on iOS .NET 10
        await FooterView.TranslateToAsync(0, 0, AnimationDuration, Easing.CubicInOut);

        Debug.WriteLine("Footer is now displayed"); // Never reached on .NET 10 iOS
    }

    private async Task HideFooter()
    {
        if (!FooterView.IsVisible)
            return;

        Debug.WriteLine("Hiding Footer");
        await FooterView.TranslateToAsync(0, FooterView.Height, AnimationDuration, Easing.CubicInOut);
        FooterView.IsVisible = false;

        Debug.WriteLine("Footer is now hidden");
    }
}