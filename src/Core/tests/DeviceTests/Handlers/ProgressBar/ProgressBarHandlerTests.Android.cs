using System;
using System.Threading.Tasks;
using Android.Content.Res;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Xunit;
using AProgressBar = Android.Widget.ProgressBar;

namespace Microsoft.Maui.DeviceTests
{
	public partial class ProgressBarHandlerTests
	{
		AProgressBar GetNativeProgressBar(ProgressBarHandler progressBarHandler) =>
			progressBarHandler.PlatformView;

		double GetNativeProgress(ProgressBarHandler progressBarHandler) =>
			(double)GetNativeProgressBar(progressBarHandler).Progress / ProgressBarExtensions.Maximum;

		Task ValidateNativeProgressColor(IProgress progressBar, Color color, Action action = null) =>
			 ValidateHasColor(progressBar, color, action);

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public Task ResetRestoresNativeTint(bool replaceDrawable) =>
			InvokeOnMainThreadAsync(async () =>
			{
				var progress = new ProgressBarStub { Progress = 0.75 };
				var handler = CreateHandler(progress);
				var native = GetNativeProgressBar(handler);
				native.ProgressTintList = ColorStateList.ValueOf(Colors.Green.ToPlatform());
				var original = native.ProgressDrawable;
				using var donor = new AProgressBar(MauiContext.Context, null, global::Android.Resource.Attribute.ProgressBarStyleHorizontal);

				progress.ProgressColor = Colors.Red;
				handler.UpdateValue(nameof(IProgress.ProgressColor));
				if (replaceDrawable)
					native.ProgressDrawable = donor.ProgressDrawable;

				await native.AssertContainsColor(Colors.Red, MauiContext);

				progress.ProgressColor = null;
				handler.UpdateValue(nameof(IProgress.ProgressColor));
				await native.AssertContainsColor(Colors.Green, MauiContext);

				native.ProgressDrawable = original;
				await native.AssertContainsColor(Colors.Green, MauiContext);
			});
	}
}