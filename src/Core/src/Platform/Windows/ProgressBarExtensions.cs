using System.Runtime.CompilerServices;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace Microsoft.Maui.Platform
{
	public static class ProgressBarExtensions
	{
		static readonly ConditionalWeakTable<ProgressBar, OriginalForeground> s_originalForegrounds = new();

		public static void UpdateProgress(this ProgressBar platformProgressBar, IProgress progress)
		{
			platformProgressBar.Value = progress.Progress;
		}

		public static void UpdateProgressColor(this ProgressBar platformProgressBar, IProgress progress)
		{
			Color progressColor = progress.ProgressColor;
			if (progressColor is not null)
			{
				if (!s_originalForegrounds.TryGetValue(platformProgressBar, out _))
				{
					var binding = platformProgressBar.GetBindingExpression(ProgressBar.ForegroundProperty)?.ParentBinding;
					s_originalForegrounds.Add(platformProgressBar, new OriginalForeground(
						platformProgressBar.ReadLocalValue(ProgressBar.ForegroundProperty), binding));
					if (binding is not null)
						platformProgressBar.ClearValue(ProgressBar.ForegroundProperty);
				}

				platformProgressBar.Foreground = progressColor.ToPlatform();
			}
			else if (s_originalForegrounds.TryGetValue(platformProgressBar, out var original))
			{
				s_originalForegrounds.Remove(platformProgressBar);
				original.Restore(platformProgressBar);
			}
		}

		sealed class OriginalForeground(object localValue, Binding? binding)
		{
			public void Restore(ProgressBar view)
			{
				if (binding is not null)
					view.SetBinding(ProgressBar.ForegroundProperty, binding);
				else if (localValue is Brush || localValue is null)
					view.SetValue(ProgressBar.ForegroundProperty, localValue);
				else
					view.ClearValue(ProgressBar.ForegroundProperty);
			}
		}
	}
}