#nullable disable
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls
{
    /// <summary>
    /// A floating action button that provides a primary action in a view.
    /// </summary>
    public partial class FloatingActionButton : View, IFloatingActionButton
    {
        /// <summary>Bindable property for <see cref="Command"/>.</summary>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command), typeof(ICommand), typeof(FloatingActionButton), null,
            propertyChanging: (bindable, oldvalue, newvalue) => ((FloatingActionButton)bindable).OnCommandChanging(),
            propertyChanged: (bindable, oldvalue, newvalue) => ((FloatingActionButton)bindable).OnCommandChanged());

        /// <summary>Bindable property for <see cref="CommandParameter"/>.</summary>
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter), typeof(object), typeof(FloatingActionButton), null,
            propertyChanged: (bindable, oldvalue, newvalue) => ((FloatingActionButton)bindable).CommandCanExecuteChanged(bindable, EventArgs.Empty));

        /// <summary>Bindable property for <see cref="Icon"/>.</summary>
        public static readonly BindableProperty IconProperty = BindableProperty.Create(
            nameof(Icon), typeof(ImageSource), typeof(FloatingActionButton), default(ImageSource));

        /// <summary>Bindable property for <see cref="Text"/>.</summary>
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text), typeof(string), typeof(FloatingActionButton), null,
            propertyChanged: (bindable, oldVal, newVal) => ((FloatingActionButton)bindable).InvalidateMeasureInternal(InvalidationTrigger.MeasureChanged));

        /// <summary>Bindable property for <see cref="Size"/>.</summary>
        public static readonly BindableProperty SizeProperty = BindableProperty.Create(
            nameof(Size), typeof(FabSize), typeof(FloatingActionButton), FabSize.Normal,
            propertyChanged: (bindable, oldVal, newVal) => ((FloatingActionButton)bindable).InvalidateMeasureInternal(InvalidationTrigger.MeasureChanged));

        /// <summary>Bindable property for <see cref="Anchor"/>.</summary>
        public static readonly BindableProperty AnchorProperty = BindableProperty.Create(
            nameof(Anchor), typeof(FabAnchor), typeof(FloatingActionButton), FabAnchor.BottomRight);

        /// <summary>Bindable property for <see cref="Elevation"/>.</summary>
        public static readonly BindableProperty ElevationProperty = BindableProperty.Create(
            nameof(Elevation), typeof(float), typeof(FloatingActionButton), 6f);

        /// <summary>Bindable property for <see cref="IconColor"/>.</summary>
        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
            nameof(IconColor), typeof(Color), typeof(FloatingActionButton), null);

        /// <summary>Bindable property for <see cref="IsExtended"/>.</summary>
        public static readonly BindableProperty IsExtendedProperty = BindableProperty.Create(
            nameof(IsExtended), typeof(bool), typeof(FloatingActionButton), false,
            propertyChanged: (bindable, oldVal, newVal) => ((FloatingActionButton)bindable).InvalidateMeasureInternal(InvalidationTrigger.MeasureChanged));

        /// <summary>Bindable property for <see cref="AutoHideOnScroll"/>.</summary>
        public static readonly BindableProperty AutoHideOnScrollProperty = BindableProperty.Create(
            nameof(AutoHideOnScroll), typeof(bool), typeof(FloatingActionButton), false);

        /// <summary>Bindable property for <see cref="CornerRadius"/>.</summary>
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
            nameof(CornerRadius), typeof(int), typeof(FloatingActionButton), -1);

        /// <summary>
        /// Occurs when the <see cref="FloatingActionButton"/> is clicked or tapped.
        /// </summary>
        public event EventHandler Clicked;

        /// <summary>
        /// Occurs when the <see cref="FloatingActionButton"/> is pressed.
        /// </summary>
        public event EventHandler Pressed;

        /// <summary>
        /// Occurs when the <see cref="FloatingActionButton"/> is released.
        /// </summary>
        public event EventHandler Released;

        /// <summary>
        /// Gets or sets the command to invoke when the FAB is clicked.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the parameter to pass to the <see cref="Command"/>.
        /// </summary>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Gets or sets the icon image source for the FAB.
        /// </summary>
        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <summary>
        /// Gets or sets the text displayed on the FAB when in extended mode.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// Gets or sets the size variant of the FAB.
        /// </summary>
        public FabSize Size
        {
            get => (FabSize)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        /// <summary>
        /// Gets or sets the anchor position of the FAB.
        /// </summary>
        public FabAnchor Anchor
        {
            get => (FabAnchor)GetValue(AnchorProperty);
            set => SetValue(AnchorProperty, value);
        }

        /// <summary>
        /// Gets or sets the elevation (shadow depth) of the FAB.
        /// </summary>
        public float Elevation
        {
            get => (float)GetValue(ElevationProperty);
            set => SetValue(ElevationProperty, value);
        }

        /// <summary>
        /// Gets or sets the color of the icon.
        /// </summary>
        public Color IconColor
        {
            get => (Color)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the FAB is in extended mode.
        /// </summary>
        public bool IsExtended
        {
            get => (bool)GetValue(IsExtendedProperty);
            set => SetValue(IsExtendedProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the FAB hides automatically on scroll.
        /// </summary>
        public bool AutoHideOnScroll
        {
            get => (bool)GetValue(AutoHideOnScrollProperty);
            set => SetValue(AutoHideOnScrollProperty, value);
        }

        /// <summary>
        /// Gets or sets the corner radius of the FAB.
        /// </summary>
        public int CornerRadius
        {
            get => (int)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #region IFloatingActionButton

        string IFloatingActionButton.Text => Text;
        FabSize IFloatingActionButton.Size => Size;
        FabAnchor IFloatingActionButton.Anchor => Anchor;
        float IFloatingActionButton.Elevation => Elevation;
        Color IFloatingActionButton.IconColor => IconColor;
        bool IFloatingActionButton.IsExtended => IsExtended;
        bool IFloatingActionButton.AutoHideOnScroll => AutoHideOnScroll;
        int IFloatingActionButton.CornerRadius => CornerRadius;

        void IFloatingActionButton.Clicked() => OnClicked();
        void IFloatingActionButton.Pressed() => OnPressed();
        void IFloatingActionButton.Released() => OnReleased();

        #endregion

        #region IImageSourcePart

        IImageSource IImageSourcePart.Source => Icon;
        bool IImageSourcePart.IsAnimationPlaying => false;
        void IImageSourcePart.UpdateIsLoading(bool isLoading) { }

        #endregion

        void OnClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);

            if (Command?.CanExecute(CommandParameter) == true)
                Command.Execute(CommandParameter);
        }

        void OnPressed()
        {
            Pressed?.Invoke(this, EventArgs.Empty);
        }

        void OnReleased()
        {
            Released?.Invoke(this, EventArgs.Empty);
        }

        void OnCommandChanged()
        {
            if (Command != null)
                Command.CanExecuteChanged += CommandCanExecuteChanged;

            CommandCanExecuteChanged(this, EventArgs.Empty);
        }

        void OnCommandChanging()
        {
            if (Command != null)
                Command.CanExecuteChanged -= CommandCanExecuteChanged;
        }

        void CommandCanExecuteChanged(object sender, EventArgs e)
        {
            if (Command != null)
                SetValue(IsEnabledProperty, Command.CanExecute(CommandParameter));
        }

        /// <summary>
        /// Shows the FAB with a scale and fade animation.
        /// </summary>
        /// <param name="duration">Animation duration in milliseconds. Default is 250ms.</param>
        public async Task ShowAsync(uint duration = 250)
        {
            if (IsVisible)
                return;

            IsVisible = true;
            Scale = 0;
            Opacity = 0;

            await Task.WhenAll(
                this.ScaleToAsync(1, duration, Easing.SpringOut),
                this.FadeToAsync(1, duration, Easing.CubicIn));
        }

        /// <summary>
        /// Hides the FAB with a scale and fade animation.
        /// </summary>
        /// <param name="duration">Animation duration in milliseconds. Default is 200ms.</param>
        public async Task HideAsync(uint duration = 200)
        {
            if (!IsVisible)
                return;

            await Task.WhenAll(
                this.ScaleToAsync(0, duration, Easing.CubicOut),
                this.FadeToAsync(0, duration, Easing.CubicOut));

            IsVisible = false;
        }

        /// <summary>
        /// Toggles between extended (icon + text) and collapsed (icon only) modes with animation.
        /// </summary>
        /// <param name="duration">Animation duration in milliseconds. Default is 200ms.</param>
        public async Task ToggleExtendedAsync(uint duration = 200)
        {
            IsExtended = !IsExtended;
            await this.ScaleToAsync(0.9, duration / 2, Easing.CubicOut);
            await this.ScaleToAsync(1.0, duration / 2, Easing.SpringOut);
        }
    }
}
