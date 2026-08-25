using System;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using WThickness = Microsoft.UI.Xaml.Thickness;
using WCornerRadius = Microsoft.UI.Xaml.CornerRadius;
using WVisibility = Microsoft.UI.Xaml.Visibility;

namespace Microsoft.Maui.Handlers
{
	public partial class FloatingActionButtonHandler : ViewHandler<IFloatingActionButton, MauiFloatingActionButton>
	{
		const double NormalSize = 56;

		PointerEventHandler? _pointerPressedHandler;
		PointerEventHandler? _pointerReleasedHandler;
		Brush? _defaultBackground;
		Brush? _defaultForeground;
		WCornerRadius _defaultCornerRadius;
		bool _isPressed;

		protected override MauiFloatingActionButton CreatePlatformView() => new MauiFloatingActionButton();

		protected override void ConnectHandler(MauiFloatingActionButton platformView)
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

		protected override void DisconnectHandler(MauiFloatingActionButton platformView)
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
			if (!VirtualView.IsExtended || string.IsNullOrEmpty(VirtualView.Text))
				return new Graphics.Size(NormalSize, NormalSize);

			var desiredSize = base.GetDesiredSize(widthConstraint, heightConstraint);
			return new Graphics.Size(Math.Max(desiredSize.Width, NormalSize), NormalSize);
		}

		public static void MapIcon(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
		{
			if (handler is FloatingActionButtonHandler fabHandler)
				fabHandler.SourceLoader.UpdateImageSourceAsync().FireAndForget(handler);
		}

		public static void MapText(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
		{
			if (handler.PlatformView is not MauiFloatingActionButton platformView)
				return;

			platformView.Label.Text = fab.Text ?? string.Empty;
			platformView.Label.Visibility = fab.IsExtended && !string.IsNullOrEmpty(fab.Text)
				? WVisibility.Visible
				: WVisibility.Collapsed;
			platformView.InvalidateMeasure();
			fab.InvalidateMeasure();
		}

		public static void MapBackground(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
		{
			if (handler.PlatformView is not MauiFloatingActionButton platformView)
				return;

			var paint = fab.Background;

			if (paint is null)
			{
				platformView.Resources.RemoveKeys(BackgroundResourceKeys);
			}
			else
			{
				var brush = Graphics.PaintExtensions.ToPlatform(paint);
				platformView.Resources.SetValueForAllKey(BackgroundResourceKeys, brush);
			}
			platformView.RefreshThemeResources();
		}

		static readonly string[] BackgroundResourceKeys =
		{
			"ButtonBackground",
			"ButtonBackgroundPointerOver",
			"ButtonBackgroundPressed",
			"ButtonBackgroundDisabled",
		};

		public static void MapIconColor(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
		{
			if (handler is not FloatingActionButtonHandler fabHandler ||
				handler.PlatformView is not MauiFloatingActionButton platformView)
				return;

			var foreground = fab.IconColor?.ToPlatform() ?? fabHandler._defaultForeground;
			platformView.Foreground = foreground;
			platformView.Label.Foreground = foreground;
		}

		public static void MapElevation(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
		{
			if (handler.PlatformView is not MauiFloatingActionButton platformView)
				return;

			var elevation = Math.Max(0, fab.Elevation);

			if (elevation > 0)
			{
				platformView.Shadow ??= new ThemeShadow();
				platformView.Translation = new Vector3(0, 0, elevation);
			}
			else
			{
				platformView.Translation = new Vector3(0, 0, 0);
			}
		}

		public static void MapCornerRadius(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
		{
			if (handler.PlatformView is not MauiFloatingActionButton platformView)
				return;

			var radius = fab.CornerRadius >= 0
				? fab.CornerRadius
				: NormalSize / 2;
			platformView.CornerRadius = new WCornerRadius(radius);
		}

		public static void MapIsExtended(IFloatingActionButtonHandler handler, IFloatingActionButton fab)
		{
			handler.UpdateValue(nameof(IFloatingActionButton.Text));
			handler.UpdateValue(nameof(IFloatingActionButton.CornerRadius));
			handler.UpdateValue(nameof(IFloatingActionButton.Background));
			handler.UpdateValue(nameof(IFloatingActionButton.IconColor));

			// Force parent to re-measure after extended state changes,
			// otherwise the first toggle won't trigger a layout pass.
			if (handler.PlatformView is MauiFloatingActionButton platformView)
			{
				platformView.Padding = fab.IsExtended ? new WThickness(16, 0, 16, 0) : new WThickness(0);
				var parent = platformView.Parent as FrameworkElement;
				parent?.InvalidateMeasure();
				parent?.InvalidateArrange();
			}
		}

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
					fabHandler.PlatformView.IconImage.Source = platformImage;
			}
		}
	}
}
