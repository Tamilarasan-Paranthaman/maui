using Android.Content;
using Android.Graphics.Drawables;
using AProgressBar = Android.Widget.ProgressBar;

namespace Microsoft.Maui.Platform;

sealed class MauiProgressBar(Context context)
	: AProgressBar(context, null, global::Android.Resource.Attribute.ProgressBarStyleHorizontal)
{
	public override Drawable? ProgressDrawable
	{
		get => base.ProgressDrawable;
		set
		{
			base.ProgressDrawable = value;
			this.RefreshProgressColorOverride();
		}
	}

	public override Drawable? IndeterminateDrawable
	{
		get => base.IndeterminateDrawable;
		set
		{
			base.IndeterminateDrawable = value;
			this.RefreshProgressColorOverride();
		}
	}

	public override bool Indeterminate
	{
		get => base.Indeterminate;
		set
		{
			base.Indeterminate = value;
			this.RefreshProgressColorOverride();
		}
	}
}
