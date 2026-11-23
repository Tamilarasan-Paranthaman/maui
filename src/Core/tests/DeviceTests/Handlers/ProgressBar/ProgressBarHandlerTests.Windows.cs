using System;
using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	public partial class ProgressBarHandlerTests
	{
		ProgressBar GetNativeProgressBar(ProgressBarHandler progressBarHandler) =>
			progressBarHandler.PlatformView;

		double GetNativeProgress(ProgressBarHandler progressBarHandler) =>
			GetNativeProgressBar(progressBarHandler).Value;

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public Task ResetRestoresPreviousForegroundSource(bool hasLocalForeground) =>
			InvokeOnMainThreadAsync(() =>
			{
				var progress = new ProgressBarStub();
				var native = new ProgressBar();
				var foregroundProperty = ProgressBar.ForegroundProperty;
				object original = DependencyProperty.UnsetValue;
				if (hasLocalForeground)
				{
					original = new SolidColorBrush(Colors.Green.ToWindowsColor());
					native.SetValue(foregroundProperty, original);
				}

				native.UpdateProgressColor(progress);
				Assert.Equal(original, native.ReadLocalValue(foregroundProperty));

				progress.ProgressColor = Colors.Red;
				native.UpdateProgressColor(progress);
				Assert.Equal(Colors.Red.ToWindowsColor(), Assert.IsType<SolidColorBrush>(native.Foreground).Color);

				progress.ProgressColor = null;
				native.UpdateProgressColor(progress);
				Assert.Equal(original, native.ReadLocalValue(foregroundProperty));
			});

		async Task ValidateNativeProgressColor(IProgress progressBar, Color color, Action action = null)
		{
			var expected = await GetValueAsync(progressBar, handler =>
			{
				var native = GetNativeProgressBar(handler);
				action?.Invoke();

				var foreground = native.Foreground;

				if (foreground is SolidColorBrush solidColorBrush)
					return solidColorBrush.Color.ToColor();

				return null;
			});
			Assert.Equal(expected, color);
		}
	}
}