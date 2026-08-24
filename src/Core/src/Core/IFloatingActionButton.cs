#nullable enable

using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{
    /// <summary>
    /// Represents a floating action button that provides a primary action in a view.
    /// </summary>
    public interface IFloatingActionButton : IView, IImageSourcePart
    {
        /// <summary>
        /// Gets the text displayed on the FAB when in extended mode.
        /// </summary>
        string? Text { get; }

        /// <summary>
        /// Gets the size variant of the floating action button.
        /// </summary>
        FabSize Size { get; }

        /// <summary>
        /// Gets the anchor position of the floating action button.
        /// </summary>
        FabAnchor Anchor { get; }

        /// <summary>
        /// Gets the elevation (shadow depth) of the floating action button.
        /// </summary>
        float Elevation { get; }

        /// <summary>
        /// Gets the color applied to the icon.
        /// </summary>
        Color? IconColor { get; }

        /// <summary>
        /// Gets a value indicating whether the FAB is in extended mode (showing text and icon).
        /// </summary>
        bool IsExtended { get; }

        /// <summary>
        /// Gets a value indicating whether the FAB should automatically hide when the user scrolls.
        /// </summary>
        bool AutoHideOnScroll { get; }

        /// <summary>
        /// Gets the corner radius of the floating action button.
        /// </summary>
        int CornerRadius { get; }

        /// <summary>
        /// Occurs when the FAB is clicked.
        /// </summary>
        void Clicked();

        /// <summary>
        /// Occurs when the FAB is pressed.
        /// </summary>
        void Pressed();

        /// <summary>
        /// Occurs when the FAB is released.
        /// </summary>
        void Released();
    }
}
