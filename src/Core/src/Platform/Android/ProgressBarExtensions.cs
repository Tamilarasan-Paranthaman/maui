using System.Runtime.CompilerServices;
using Android.Graphics.Drawables;
using Google.Android.Material.ProgressIndicator;
using Microsoft.Maui.Graphics;
using AColorFilter = Android.Graphics.ColorFilter;
using AProgressBar = Android.Widget.ProgressBar;

namespace Microsoft.Maui.Platform
{
	public static class ProgressBarExtensions
	{
		public const int Maximum = 10000;

		static readonly ConditionalWeakTable<AProgressBar, ProgressColorOverride> s_progressColorOverrides = new();

		public static void UpdateProgress(this AProgressBar platformProgressBar, IProgress progress)
		{
			platformProgressBar.Progress = (int)(progress.Progress * Maximum);
		}

		public static void UpdateProgressColor(this AProgressBar platformProgressBar, IProgress progress)
		{
			if (platformProgressBar is LinearProgressIndicator materialProgressBar)
			{
				materialProgressBar.UpdateLinearProgressIndicatorColor(progress);
			}
			else
			{
				platformProgressBar.UpdateProgressBarColor(progress);
			}
		}

		static void UpdateProgressBarColor(this AProgressBar platformProgressBar, IProgress progress)
		{
			if (progress.ProgressColor is { } color)
			{
				var state = s_progressColorOverrides.GetValue(platformProgressBar, static _ => new ProgressColorOverride());
				state.Apply(platformProgressBar, color);
			}
			else if (s_progressColorOverrides.TryGetValue(platformProgressBar, out var state))
			{
				s_progressColorOverrides.Remove(platformProgressBar);
				state.Restore();
			}
		}

		internal static void RefreshProgressColorOverride(this MauiProgressBar platformProgressBar)
		{
			if (s_progressColorOverrides.TryGetValue(platformProgressBar, out var state))
				state.Apply(platformProgressBar, state.Color);
		}

		sealed class ProgressColorOverride
		{
			Drawable? _drawable;
			AColorFilter? _originalFilter;

			public Color Color { get; private set; } = null!;

			public void Apply(AProgressBar platformProgressBar, Color color)
			{
				Color = color;
				bool indeterminate = platformProgressBar.Indeterminate;
				var drawable = indeterminate ? platformProgressBar.IndeterminateDrawable : platformProgressBar.ProgressDrawable;
				if (!indeterminate && drawable is LayerDrawable layers)
					drawable = layers.FindDrawableByLayerId(global::Android.Resource.Id.Progress) ?? drawable;

				if (_drawable != drawable)
				{
					Restore();
					_drawable = drawable?.Mutate();
					_originalFilter = _drawable?.GetColorFilter();
				}

				// A filter overrides the color without replacing the drawable's native tint.
				_drawable?.SetColorFilter(color, FilterMode.SrcIn);
			}

			public void Restore() => _drawable?.SetColorFilter(_originalFilter);
		}

		static void UpdateLinearProgressIndicatorColor(this LinearProgressIndicator materialProgressBar, IProgress progress)
		{
			Color color = progress.ProgressColor;

			if (color is null)
			{
				// Reset to theme default by passing empty array - Material3's setIndicatorColor() 
				// automatically resolves this to theme's colorPrimary when length == 0
				materialProgressBar.SetIndicatorColor([]);
			}
			else
			{
				var colorArray = new int[] { color.ToPlatform() };
				materialProgressBar.SetIndicatorColor(colorArray);
			}
		}
	}
}