using Android.Content.Res;
using Google.Android.Material.AppBar;
using Microsoft.Maui.Handlers;
using AView = Android.Views.View;

namespace Maui.Controls.Sample.Platform
{
    public static class ToolbarExtensions
    {
        public static void MapStatusBarColor(IToolbarHandler handler, IToolbar toolbar)
        {
            if (handler.PlatformView is MaterialToolbar materialToolbar)
            {
                // Wire into the ViewAttachedToWindow event to set the color when the toolbar is ready
                materialToolbar.ViewAttachedToWindow += OnToolbarAttachedToWindow;
            }
        }

        static void OnToolbarAttachedToWindow(object? sender, AView.ViewAttachedToWindowEventArgs e)
        {
            if (sender is MaterialToolbar toolbar)
            {
                // Unwire the event to prevent multiple calls
                toolbar.ViewAttachedToWindow -= OnToolbarAttachedToWindow;

                // Apply the status bar (AppBarLayout) color
                ApplyAppBarLayoutColor(toolbar);
            }
        }

        static void ApplyAppBarLayoutColor(MaterialToolbar toolbar)
        {
            var parent = toolbar.Parent;

            // Walk up the parent hierarchy to find the AppBarLayout
            while (parent is not null && parent is not AppBarLayout)
            {
                parent = parent.Parent;
            }

            var appBarLayout = parent as AppBarLayout;

            if (appBarLayout is not null)
            {
                var backgroundColor = Android.Graphics.Color.YellowGreen;

                // Apply the background color
                appBarLayout.SetBackgroundColor(backgroundColor);
            }
        }
    }
}