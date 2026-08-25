#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIButton;
#elif MONOANDROID
using PlatformView = Google.Android.Material.FloatingActionButton.ExtendedFloatingActionButton;
#elif WINDOWS
using PlatformView = Microsoft.Maui.Platform.MauiFloatingActionButton;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

using Microsoft.Maui.Platform;

namespace Microsoft.Maui.Handlers
{
    public partial class FloatingActionButtonHandler : IFloatingActionButtonHandler
    {
        public static IPropertyMapper<IFloatingActionButton, IFloatingActionButtonHandler> Mapper =
            new PropertyMapper<IFloatingActionButton, IFloatingActionButtonHandler>(ViewHandler.ViewMapper)
            {
                [nameof(IFloatingActionButton.Source)] = MapIcon,
                [nameof(IFloatingActionButton.Text)] = MapText,
                [nameof(IFloatingActionButton.Background)] = MapBackground,
                [nameof(IFloatingActionButton.IconColor)] = MapIconColor,
                [nameof(IFloatingActionButton.Elevation)] = MapElevation,
                [nameof(IFloatingActionButton.CornerRadius)] = MapCornerRadius,
                [nameof(IFloatingActionButton.IsExtended)] = MapIsExtended,
            };

        public static CommandMapper<IFloatingActionButton, IFloatingActionButtonHandler> CommandMapper =
            new(ViewHandler.ViewCommandMapper)
            {
            };

        ImageSourcePartLoader? _imageSourcePartLoader;

        public virtual ImageSourcePartLoader SourceLoader =>
            _imageSourcePartLoader ??= new ImageSourcePartLoader(new FabImageSourcePartSetter(this));

        public FloatingActionButtonHandler() : base(Mapper, CommandMapper)
        {
        }

        public FloatingActionButtonHandler(IPropertyMapper? mapper)
            : base(mapper ?? Mapper, CommandMapper)
        {
        }

        public FloatingActionButtonHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
            : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
        {
        }

        IFloatingActionButton IFloatingActionButtonHandler.VirtualView => VirtualView;

        PlatformView IFloatingActionButtonHandler.PlatformView => PlatformView;

        partial class FabImageSourcePartSetter : ImageSourcePartSetter<IFloatingActionButtonHandler>
        {
            public FabImageSourcePartSetter(IFloatingActionButtonHandler handler)
                : base(handler)
            {
            }
        }
    }
}
