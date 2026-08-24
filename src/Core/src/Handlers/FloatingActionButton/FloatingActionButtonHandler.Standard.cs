using System;

namespace Microsoft.Maui.Handlers
{
    public partial class FloatingActionButtonHandler : ViewHandler<IFloatingActionButton, object>
    {
        protected override object CreatePlatformView() => throw new NotImplementedException();

        public static void MapIcon(IFloatingActionButtonHandler handler, IFloatingActionButton fab) { }
        public static void MapText(IFloatingActionButtonHandler handler, IFloatingActionButton fab) { }
        public static void MapSize(IFloatingActionButtonHandler handler, IFloatingActionButton fab) { }
        public static void MapBackground(IFloatingActionButtonHandler handler, IFloatingActionButton fab) { }
        public static void MapIconColor(IFloatingActionButtonHandler handler, IFloatingActionButton fab) { }
        public static void MapElevation(IFloatingActionButtonHandler handler, IFloatingActionButton fab) { }
        public static void MapCornerRadius(IFloatingActionButtonHandler handler, IFloatingActionButton fab) { }
        public static void MapIsExtended(IFloatingActionButtonHandler handler, IFloatingActionButton fab) { }

        partial class FabImageSourcePartSetter
        {
            public override void SetImageSource(object? platformImage) { }
        }
    }
}
