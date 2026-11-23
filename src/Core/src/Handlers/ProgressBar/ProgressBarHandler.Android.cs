using AndroidProgressBar = Android.Widget.ProgressBar;

namespace Microsoft.Maui.Handlers
{
	public partial class ProgressBarHandler : ViewHandler<IProgress, AndroidProgressBar>
	{
		protected override AndroidProgressBar CreatePlatformView()
		{
			return new MauiProgressBar(Context)
			{
				Indeterminate = false,
				Max = ProgressBarExtensions.Maximum
			};
		}

		public static void MapProgress(IProgressBarHandler handler, IProgress progress)
		{
			handler.PlatformView?.UpdateProgress(progress);
		}

		public static void MapProgressColor(IProgressBarHandler handler, IProgress progress)
		{
			handler.PlatformView?.UpdateProgressColor(progress);
		}
	}
}