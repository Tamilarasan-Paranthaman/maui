using System;
using System.Threading.Tasks;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Views;
using Google.Android.Material.FloatingActionButton;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Primitives;

namespace Microsoft.Maui.Handlers
{
    public partial class FloatingActionButtonHandler : ViewHandler<IFloatingActionButton, ExtendedFloatingActionButton>
    {
        const float DefaultSize = 56f;
        const float DefaultIconSize = 24f;

        // dp to px conversion
        float DpToPx(float dp) => Context.ToPixels(dp);

        protected override ExtendedFloatingActionButton CreatePlatformView()
        {
            var fab = new ExtendedFloatingActionButton(Context);
            fab.IconSize = (int)DpToPx(DefaultIconSize);
            fab.Shrink();
            return fab;
        }

        public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (!VirtualView.IsExtended &&
                !Dimension.IsExplicitSet(VirtualView.Width) &&
                !Dimension.IsExplicitSet(VirtualView.Height))
            {
                return new Size(DefaultSize, DefaultSize);
            }

            // Extended FABs and explicitly sized collapsed FABs use standard MAUI measurement.
            return base.GetDesiredSize(widthConstraint, heightConstraint);
        }

        protected override void ConnectHandler(ExtendedFloatingActionButton platformView)
        {
            base.ConnectHandler(platformView);
            platformView.Click += OnClick;
            platformView.Touch += OnTouch;
        }

        protected override void DisconnectHandler(ExtendedFloatingActionButton platformView)
        {
            platformView.Click -= OnClick;
            platformView.Touch -= OnTouch;
            base.DisconnectHandler(platformView);
            SourceLoader.Reset();
        }

        public static void MapIcon(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler is FloatingActionButtonHandler fabHandler)
            {
                MapIconAsync(fabHandler).FireAndForget(handler);
            }
        }

        static Task MapIconAsync(FloatingActionButtonHandler handler) =>
            handler.SourceLoader.UpdateImageSourceAsync();

        public static void MapText(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler.PlatformView is not ExtendedFloatingActionButton platformFab)
                return;

            if (fab.IsExtended)
            {
                platformFab.Text = fab.Text ?? string.Empty;
            }
            platformFab.ContentDescription = fab.Text;
        }

        public static void MapBackground(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler.PlatformView is not ExtendedFloatingActionButton platformFab)
                return;

            var background = fab.Background;
            if (background is SolidPaint solidPaint && solidPaint.Color is Color color)
            {
                platformFab.BackgroundTintList = ColorStateList.ValueOf(color.ToPlatform());
            }
        }

        public static void MapIconColor(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler.PlatformView is not ExtendedFloatingActionButton platformFab)
                return;

            if (fab.IconColor is Color iconColor)
            {
                platformFab.IconTint = ColorStateList.ValueOf(iconColor.ToPlatform());
            }
        }

        public static void MapElevation(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler.PlatformView is not ExtendedFloatingActionButton platformFab)
                return;

            platformFab.Elevation = fab.Elevation;
        }

        public static void MapCornerRadius(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            // ExtendedFloatingActionButton uses pill/circular shape by default
        }

        public static void MapIsExtended(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler.PlatformView is not ExtendedFloatingActionButton platformFab)
                return;

            if (fab.IsExtended)
            {
                // Set text before extending so it shows
                platformFab.Text = fab.Text ?? string.Empty;
                platformFab.Extend();
            }
            else
            {
                platformFab.Shrink();
                platformFab.Post(() => platformFab.Text = string.Empty);
            }

            // Force MAUI to re-measure with new size
            fab.InvalidateMeasure();
        }

        void OnClick(object? sender, EventArgs e)
        {
            VirtualView?.Clicked();
        }

        void OnTouch(object? sender, View.TouchEventArgs e)
        {
            if (e.Event?.Action == MotionEventActions.Down)
            {
                VirtualView?.Pressed();
            }
            else if (e.Event?.Action == MotionEventActions.Up || e.Event?.Action == MotionEventActions.Cancel)
            {
                VirtualView?.Released();
            }

            e.Handled = false;
        }

        partial class FabImageSourcePartSetter
        {
            public override void SetImageSource(Drawable? platformImage)
            {
                if (Handler?.PlatformView is not ExtendedFloatingActionButton fab)
                    return;

                fab.Icon = platformImage;
            }
        }
    }
}
