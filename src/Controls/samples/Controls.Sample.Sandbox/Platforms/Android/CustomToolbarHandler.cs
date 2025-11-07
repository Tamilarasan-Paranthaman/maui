using Google.Android.Material.AppBar;
using Microsoft.Maui.Handlers;
using AView = Android.Views.View;
using Android.Views;
using AndroidX.Core.View;
using Android.OS;
using Android.Provider;
using Android.App;

namespace Maui.Controls.Sample.Platform
{
    public static class ToolbarExtensions
    {
        public static void MapSystemBarColor(IToolbarHandler handler, IToolbar toolbar)
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

                // Apply the navigation bar color
                ApplyNavigationBarColor(toolbar);
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
                var backgroundColor = Android.Graphics.Color.ParseColor("#bda80b");

                // Apply the background color
                appBarLayout.SetBackgroundColor(backgroundColor);
            }
        }

        static void ApplyNavigationBarColor(MaterialToolbar toolbar)
        {
            var activity = toolbar.Context as Activity;

            if (activity?.Window is null)
            {
                return;
            }

            // Modern approach following Android 15+ (API 35+) recommendations
            // Draw proper background behind WindowInsets.Type.navigationBars()
            if (Build.VERSION.SdkInt >= BuildVersionCodes.VanillaIceCream)
            {
                // Set up window insets listener to handle navigation bar area
                ViewCompat.SetOnApplyWindowInsetsListener(activity.Window.DecorView, new ModernNavigationBarInsetsListener());
            }
        }
    }

    // Modern insets listener that properly handles navigation bar background
    // following Android 15+ (API 35+) guidelines. This will add a view with a background behind the navigation bar area.
    public class ModernNavigationBarInsetsListener : Java.Lang.Object, IOnApplyWindowInsetsListener
    {
        public WindowInsetsCompat? OnApplyWindowInsets(AView? v, WindowInsetsCompat? insets)
        {
            if (insets is null || v?.Context is null)
            {
                return insets;
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                // Get navigation bar insets - this is the modern approach
                var navigationBarInsets = insets.GetInsets(WindowInsetsCompat.Type.NavigationBars());

                if (v is ViewGroup rootView && navigationBarInsets != null && navigationBarInsets.Bottom > 0)
                {
                    // Check if navigation bar background view already exists
                    AView? existingNavigationBarBackground = null;

                    for (int index = 0; index < rootView.ChildCount; index++)
                    {
                        var child = rootView.GetChildAt(index);
                        if (child?.Tag?.ToString() == "NavigationBarBackground")
                        {
                            existingNavigationBarBackground = child;
                            break;
                        }
                    }

                    if (existingNavigationBarBackground is null)
                    {
                        // Create a dedicated view for the navigation bar background only if it doesn't exist
                        var navigationBarBackground = new AView(v.Context);
                        navigationBarBackground.SetBackgroundColor(Android.Graphics.Color.ParseColor("#bda80b"));
                        navigationBarBackground.Tag = "NavigationBarBackground";

                        // Position the navigation bar background at the bottom
                        var layoutParams = new Android.Widget.FrameLayout.LayoutParams(
                            ViewGroup.LayoutParams.MatchParent,
                            navigationBarInsets.Bottom,
                            GravityFlags.Bottom
                        );

                        // Add the navigation bar background view
                        rootView.AddView(navigationBarBackground, layoutParams);
                    }
                    else
                    {
                        // Update existing view's height if insets changed
                        if (existingNavigationBarBackground.LayoutParameters is Android.Widget.FrameLayout.LayoutParams existingParams)
                        {
                            if (existingParams.Height != navigationBarInsets.Bottom)
                            {
                                existingParams.Height = navigationBarInsets.Bottom;
                                existingNavigationBarBackground.LayoutParameters = existingParams;
                            }
                        }
                    }
                }
            }

            return insets;
        }
    }
}