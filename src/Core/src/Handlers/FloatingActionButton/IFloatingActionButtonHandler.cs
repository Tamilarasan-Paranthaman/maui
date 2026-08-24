#if __IOS__ || MACCATALYST
using PlatformView = UIKit.UIButton;
#elif MONOANDROID
using PlatformView = Google.Android.Material.FloatingActionButton.ExtendedFloatingActionButton;
#elif WINDOWS
using PlatformView = Microsoft.Maui.Platform.MauiFloatingActionButton;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif

namespace Microsoft.Maui.Handlers
{
    public interface IFloatingActionButtonHandler : IViewHandler
    {
        new IFloatingActionButton VirtualView { get; }
        new PlatformView PlatformView { get; }
    }
}
