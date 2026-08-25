using System;
using System.Threading.Tasks;
using CoreGraphics;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Primitives;
using UIKit;

namespace Microsoft.Maui.Handlers
{
    public partial class FloatingActionButtonHandler : ViewHandler<IFloatingActionButton, UIButton>
    {
        const float NormalSize = 56f;
        const float IconTextSpacing = 8f;

        readonly FabProxy _proxy = new();

        protected override UIButton CreatePlatformView()
        {
            var button = new UIButton(UIButtonType.Custom);
            button.ClipsToBounds = false;

            // Make it circular
            UpdateButtonShape(button, NormalSize);

            // Configure shadow for elevation
            button.Layer.ShadowColor = UIColor.Black.CGColor;
            button.Layer.ShadowOffset = new CGSize(0, 4);
            button.Layer.ShadowRadius = 8;
            button.Layer.ShadowOpacity = 0.3f;

            return button;
        }

        protected override void ConnectHandler(UIButton platformView)
        {
            _proxy.Connect(VirtualView, platformView);
            base.ConnectHandler(platformView);
        }

        public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (Dimension.IsExplicitSet(VirtualView.Width) || Dimension.IsExplicitSet(VirtualView.Height))
                return base.GetDesiredSize(widthConstraint, heightConstraint);

            var height = NormalSize;

            if (VirtualView.IsExtended && !string.IsNullOrEmpty(VirtualView.Text))
            {
                // Measure text width + icon + padding for extended mode
                var button = PlatformView;
                var textSize = button.TitleLabel?.IntrinsicContentSize ?? CGSize.Empty;
                var iconWidth = height * 0.45f; // Icon size
                var padding = 24f; // Horizontal padding (12 each side)
                var width = iconWidth + IconTextSpacing + (float)textSize.Width + padding;
                return new Size(Math.Max(width, height), height);
            }

            return new Size(height, height);
        }

        public override void PlatformArrange(Rect frame)
        {
            base.PlatformArrange(frame);

            if (VirtualView.CornerRadius < 0)
                PlatformView.Layer.CornerRadius = (float)Math.Min(frame.Width, frame.Height) / 2f;
        }

        protected override void DisconnectHandler(UIButton platformView)
        {
            _proxy.Disconnect(platformView);
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
            if (handler.PlatformView is not UIButton button)
                return;

            if (fab.IsExtended && !string.IsNullOrEmpty(fab.Text))
            {
                button.SetTitle(fab.Text, UIControlState.Normal);
                button.SetTitleColor(new UIColor(1, 1, 1, 1), UIControlState.Normal);
                button.SetTitleColor(new UIColor(1, 1, 1, 1), UIControlState.Highlighted);
                if (fab.IconColor is Color iconClr)
                {
                    var platformColor = iconClr.ToPlatform();
                    if (platformColor is not null)
                    {
                        button.SetTitleColor(platformColor, UIControlState.Normal);
                        button.SetTitleColor(platformColor, UIControlState.Highlighted);
                    }
                }
#nullable disable
                button.TitleLabel.Font = UIFont.SystemFontOfSize(16, UIFontWeight.Medium);
#nullable enable
                button.TitleEdgeInsets = new UIEdgeInsets(0, IconTextSpacing, 0, -IconTextSpacing);

                // Pill shape for extended mode
                button.Layer.CornerRadius = NormalSize / 2f;
            }
            else
            {
                button.SetTitle(null, UIControlState.Normal);
                button.TitleEdgeInsets = UIEdgeInsets.Zero;
            }

            // Trigger re-measure when text changes in extended mode
            if (handler is FloatingActionButtonHandler fabHandler)
            {
                fabHandler.VirtualView?.InvalidateMeasure();
            }
        }

        public static void MapBackground(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler.PlatformView is not UIButton button)
                return;

            var background = fab.Background;
            if (background is SolidPaint solidPaint && solidPaint.Color is Color color)
            {
                button.BackgroundColor = color.ToPlatform();
            }
        }

        public static void MapIconColor(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler.PlatformView is not UIButton button)
                return;

            if (fab.IconColor is Color iconColor)
            {
                button.TintColor = iconColor.ToPlatform();
                button.ImageView!.TintColor = iconColor.ToPlatform();
            }
        }

        public static void MapElevation(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler.PlatformView is not UIButton button)
                return;

            // Map elevation to shadow properties
            var elevation = fab.Elevation;
            button.Layer.ShadowOffset = new CGSize(0, elevation * 0.5f);
            button.Layer.ShadowRadius = elevation;
            button.Layer.ShadowOpacity = elevation > 0 ? 0.3f : 0f;
        }

        public static void MapCornerRadius(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler.PlatformView is not UIButton button)
                return;

            if (fab.CornerRadius >= 0)
            {
                button.Layer.CornerRadius = fab.CornerRadius;
            }
        }

        public static void MapIsExtended(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            handler.UpdateValue(nameof(IFloatingActionButton.Text));

            // Update corner radius for pill shape vs circle
            if (handler.PlatformView is UIButton button)
            {
                button.Layer.CornerRadius = NormalSize / 2f;
            }
        }

        static void UpdateButtonShape(UIButton button, float size)
        {
            button.Layer.CornerRadius = size / 2f;
        }

        partial class FabImageSourcePartSetter
        {
            public override void SetImageSource(UIImage? platformImage)
            {
                if (Handler?.PlatformView is not UIButton button)
                    return;

                if (platformImage is not null)
                {
                    // Use AlwaysTemplate to allow tint color to work
                    platformImage = platformImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

                    // Scale image to fit within the button with padding
                    var iconSize = NormalSize * 0.45f; // Icon should be ~45% of button size
                    var imageSize = new CGSize(iconSize, iconSize);

                    UIGraphics.BeginImageContextWithOptions(imageSize, false, 0);
                    platformImage.Draw(new CGRect(0, 0, imageSize.Width, imageSize.Height));
                    var scaledImage = UIGraphics.GetImageFromCurrentImageContext();
                    UIGraphics.EndImageContext();

                    if (scaledImage is not null)
                    {
                        platformImage = scaledImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                    }
                }

                button.SetImage(platformImage, UIControlState.Normal);
                button.SetImage(platformImage, UIControlState.Highlighted);
                button.ImageView!.ContentMode = UIViewContentMode.Center;

                // Center the image within the button
                button.ContentMode = UIViewContentMode.Center;
                button.ImageView.ClipsToBounds = true;
            }
        }

        class FabProxy
        {
            WeakReference<IFloatingActionButton>? _virtualView;

            IFloatingActionButton? VirtualView => _virtualView is not null && _virtualView.TryGetTarget(out var v) ? v : null;

            public void Connect(IFloatingActionButton virtualView, UIButton platformView)
            {
                _virtualView = new(virtualView);
                platformView.TouchUpInside += OnTouchUpInside;
                platformView.TouchUpOutside += OnTouchUpOutside;
                platformView.TouchCancel += OnTouchCancel;
                platformView.TouchDown += OnTouchDown;
            }

            public void Disconnect(UIButton platformView)
            {
                platformView.TouchUpInside -= OnTouchUpInside;
                platformView.TouchUpOutside -= OnTouchUpOutside;
                platformView.TouchCancel -= OnTouchCancel;
                platformView.TouchDown -= OnTouchDown;
                platformView.Transform = CGAffineTransform.MakeIdentity();
            }

            void OnTouchUpInside(object? sender, EventArgs e)
            {
                AnimateReleased(sender as UIButton);

                if (VirtualView is IFloatingActionButton fab)
                {
                    fab.Released();
                    fab.Clicked();
                }
            }

            void OnTouchUpOutside(object? sender, EventArgs e)
            {
                AnimateReleased(sender as UIButton);
                VirtualView?.Released();
            }

            void OnTouchCancel(object? sender, EventArgs e)
            {
                AnimateReleased(sender as UIButton);
                VirtualView?.Released();
            }

            void OnTouchDown(object? sender, EventArgs e)
            {
                if (sender is UIButton button)
                {
                    UIView.Animate(0.1, () =>
                        button.Transform = CGAffineTransform.MakeScale(0.92f, 0.92f));
                }

                VirtualView?.Pressed();
            }

            static void AnimateReleased(UIButton? button)
            {
                if (button is null)
                    return;

                UIView.Animate(0.2, () =>
                    button.Transform = CGAffineTransform.MakeIdentity());
            }
        }
    }
}
