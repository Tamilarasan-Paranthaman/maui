using System;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace Microsoft.Maui.Handlers
{
    public partial class FloatingActionButtonHandler : ViewHandler<IFloatingActionButton, Button>
    {
        const double NormalSize = 56;
        const double MiniSize = 40;

        readonly Image _image = new()
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Stretch = Stretch.Uniform,
        };

        readonly TextBlock _text = new()
        {
            VerticalAlignment = VerticalAlignment.Center,
            Visibility = Visibility.Collapsed,
        };

        PointerEventHandler? _pointerPressedHandler;
        PointerEventHandler? _pointerReleasedHandler;
        Brush? _defaultBackground;
        Brush? _defaultForeground;
        CornerRadius _defaultCornerRadius;
        bool _isPressed;

        protected override Button CreatePlatformView()
        {
            var content = new Grid
            {
                ColumnSpacing = 8,
            };

            content.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            content.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            content.Children.Add(_image);
            content.Children.Add(_text);
            Grid.SetColumn(_text, 1);

            return new Button
            {
                Content = content,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = new Thickness(0),
                Shadow = new ThemeShadow(),
            };
        }

        protected override void ConnectHandler(Button platformView)
        {
            _defaultBackground = platformView.Background;
            _defaultForeground = platformView.Foreground;
            _defaultCornerRadius = platformView.CornerRadius;

            _pointerPressedHandler = new PointerEventHandler(OnPointerPressed);
            _pointerReleasedHandler = new PointerEventHandler(OnPointerReleased);

            platformView.Click += OnClick;
            platformView.Unloaded += OnUnloaded;
            platformView.AddHandler(UIElement.PointerPressedEvent, _pointerPressedHandler, true);
            platformView.AddHandler(UIElement.PointerReleasedEvent, _pointerReleasedHandler, true);

            base.ConnectHandler(platformView);
        }

        protected override void DisconnectHandler(Button platformView)
        {
            platformView.Click -= OnClick;
            platformView.Unloaded -= OnUnloaded;
            platformView.RemoveHandler(UIElement.PointerPressedEvent, _pointerPressedHandler);
            platformView.RemoveHandler(UIElement.PointerReleasedEvent, _pointerReleasedHandler);

            _pointerPressedHandler = null;
            _pointerReleasedHandler = null;
            _isPressed = false;

            base.DisconnectHandler(platformView);
            SourceLoader.Reset();
        }

        public override Graphics.Size GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            var size = GetButtonSize(VirtualView.Size);
            if (!VirtualView.IsExtended || string.IsNullOrEmpty(VirtualView.Text))
                return new Graphics.Size(size, size);

            var desiredSize = base.GetDesiredSize(widthConstraint, heightConstraint);
            return new Graphics.Size(Math.Max(desiredSize.Width, size), size);
        }

        public static void MapIcon(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler is FloatingActionButtonHandler fabHandler)
                fabHandler.SourceLoader.UpdateImageSourceAsync().FireAndForget(handler);
        }

        public static void MapText(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler is not FloatingActionButtonHandler fabHandler)
                return;

            fabHandler._text.Text = fab.Text ?? string.Empty;
            fabHandler._text.Visibility = fab.IsExtended && !string.IsNullOrEmpty(fab.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
            fab.InvalidateMeasure();
        }

        public static void MapSize(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler is not FloatingActionButtonHandler fabHandler)
                return;

            var size = GetButtonSize(fab.Size);
            fabHandler.PlatformView.Height = size;
            fabHandler.PlatformView.Width = fab.IsExtended ? double.NaN : size;
            fabHandler.PlatformView.MinHeight = size;
            fabHandler.PlatformView.MinWidth = size;
            fabHandler._image.Width = fab.Size == FabSize.Mini ? 18 : 24;
            fabHandler._image.Height = fab.Size == FabSize.Mini ? 18 : 24;
            fabHandler.PlatformView.Padding = fab.IsExtended ? new Thickness(16, 0, 16, 0) : new Thickness(0);
        }

        public static void MapBackground(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler is FloatingActionButtonHandler fabHandler)
                fabHandler.PlatformView.UpdateBackground(fab.Background, fabHandler._defaultBackground);
        }

        public static void MapIconColor(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler is not FloatingActionButtonHandler fabHandler)
                return;

            var foreground = fab.IconColor?.ToPlatform() ?? fabHandler._defaultForeground;
            fabHandler.PlatformView.Foreground = foreground;
            fabHandler._text.Foreground = foreground;
        }

        public static void MapElevation(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler.PlatformView is Button button)
                button.Translation = new Vector3(0, 0, Math.Max(0, fab.Elevation));
        }

        public static void MapCornerRadius(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            if (handler is not FloatingActionButtonHandler fabHandler)
                return;

            var radius = fab.CornerRadius >= 0
                ? fab.CornerRadius
                : GetButtonSize(fab.Size) / 2;
            fabHandler.PlatformView.CornerRadius = fab.CornerRadius >= 0
                ? new CornerRadius(radius)
                : fabHandler._defaultCornerRadius == default
                    ? new CornerRadius(radius)
                    : fabHandler._defaultCornerRadius;
        }

        public static void MapIsExtended(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
        {
            handler.UpdateValue(nameof(IFloatingActionButton.Text));
            handler.UpdateValue(nameof(IFloatingActionButton.Size));
            handler.UpdateValue(nameof(IFloatingActionButton.CornerRadius));
        }

        static double GetButtonSize(FabSize size) => size == FabSize.Mini ? MiniSize : NormalSize;

        void OnClick(object sender, RoutedEventArgs e) => VirtualView?.Clicked();

        void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _isPressed = true;
            VirtualView?.Pressed();
        }

        void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _isPressed = false;
            VirtualView?.Released();
        }

        void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_isPressed)
            {
                _isPressed = false;
                VirtualView?.Released();
            }
        }

        partial class FabImageSourcePartSetter
        {
            public override void SetImageSource(ImageSource? platformImage)
            {
                if (Handler is FloatingActionButtonHandler fabHandler)
                    fabHandler._image.Source = platformImage;
            }
        }
    }
}