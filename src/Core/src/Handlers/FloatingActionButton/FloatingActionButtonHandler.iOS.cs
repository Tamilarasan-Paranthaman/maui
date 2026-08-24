using System;
using System.Threading.Tasks;
using CoreGraphics;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using UIKit;

namespace Microsoft.Maui.Handlers
{
    public partial class FloatingActionButtonHandler : ViewHandler<IFloatingActionButton, UIButton>
    {
        const float NormalSize = 56f;
        const float MiniSize = 40f;

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
            var height = VirtualView.Size switch
            {
                FabSize.Mini => MiniSize,
                FabSize.Normal => NormalSize,
                _ => NormalSize
            };

            if (VirtualView.IsExtended && !string.IsNullOrEmpty(VirtualView.Text))
            {
                // Measure text width + icon + padding for extended mode
                var button = PlatformView;
                var textSize = button.TitleLabel?.IntrinsicContentSize ?? CGSize.Empty;
                var iconWidth = height * 0.45f; // Icon size
                var padding = 24f; // Horizontal padding (12 each side)
                var spacing = 8f; // Space between icon and text
                var width = iconWidth + spacing + (float)textSize.Width + padding;
                return new Size(Math.Max(width, height), height);
            }

            return new Size(height, height);
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
                if (fab.IconColor is Color iconClr)
                {
                    var platformColor = iconClr.ToPlatform();
                    if (platformColor is not null)
                        button.SetTitleColor(platformColor, UIControlState.Normal);
                }
#nullable disable
                button.TitleLabel.Font = UIFont.SystemFontOfSize(16, UIFontWeight.Medium);
#nullable enable

                // Pill shape for extended mode
                var height = fab.Size == FabSize.Mini ? MiniSize : NormalSize;
                button.Layer.CornerRadius = height / 2f;
            }
            else
            {
                button.SetTitle(null, UIControlState.Normal);
            }

            // Trigger re-measure when text changes in extended mode
            if (handler is FloatingActionButtonHandler fabHandler)
            {
                fabHandler.VirtualView?.InvalidateMeasure();
            }
        }

        public static void MapSize(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler.PlatformView is not UIButton button)
                return;

            var size = fab.Size switch
            {
                FabSize.Mini => MiniSize,
                FabSize.Normal => NormalSize,
                FabSize.Extended => NormalSize, // Height stays normal for extended
                _ => NormalSize
            };

            UpdateButtonShape(button, size);
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
            handler.UpdateValue(nameof(IFloatingActionButton.Size));

            // Update corner radius for pill shape vs circle
            if (handler.PlatformView is UIButton button)
            {
                var height = fab.Size == FabSize.Mini ? MiniSize : NormalSize;
                button.Layer.CornerRadius = height / 2f;
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
                    var fabSize = Handler.VirtualView?.Size ?? FabSize.Normal;
                    var buttonSize = fabSize == FabSize.Mini ? MiniSize : NormalSize;
                    var iconSize = buttonSize * 0.45f; // Icon should be ~45% of button size
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
                platformView.TouchDown += OnTouchDown;
            }

            public void Disconnect(UIButton platformView)
            {
                platformView.TouchUpInside -= OnTouchUpInside;
                platformView.TouchDown -= OnTouchDown;
            }

            void OnTouchUpInside(object? sender, EventArgs e)
            {
                if (VirtualView is IFloatingActionButton fab)
                {
                    fab.Released();
                    fab.Clicked();
                }
            }

            void OnTouchDown(object? sender, EventArgs e)
            {
                VirtualView?.Pressed();
            }
        }
    }
}
