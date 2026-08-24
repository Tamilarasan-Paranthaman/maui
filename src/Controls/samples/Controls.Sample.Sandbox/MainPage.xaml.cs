namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	int _tapCount;

	public MainPage()
	{
		InitializeComponent();
	}

	// FAB click
	void OnFabClicked(object? sender, EventArgs e)
	{
		_tapCount++;
		StatusLabel.Text = $"FAB tapped! Count: {_tapCount}";
	}

	// Test 1: Show/Hide animations
	async void OnHideFab(object? sender, EventArgs e)
	{
		await MainFab.HideAsync();
		StatusLabel.Text = "FAB hidden with scale+fade animation";
	}

	async void OnShowFab(object? sender, EventArgs e)
	{
		await MainFab.ShowAsync();
		StatusLabel.Text = "FAB shown with spring animation";
	}

	// Test 2: Toggle Extended
	async void OnToggleExtended(object? sender, EventArgs e)
	{
		await MainFab.ToggleExtendedAsync();
		StatusLabel.Text = $"IsExtended = {MainFab.IsExtended}";
	}

	// Test 3: Size switching
	void OnSetNormal(object? sender, EventArgs e)
	{
		MainFab.Size = Microsoft.Maui.FabSize.Normal;
		StatusLabel.Text = "Size = Normal (56dp)";
	}

	void OnSetMini(object? sender, EventArgs e)
	{
		MainFab.Size = Microsoft.Maui.FabSize.Mini;
		StatusLabel.Text = "Size = Mini (40dp)";
	}

	// Test 4: Dynamic color/elevation
	void OnSetRed(object? sender, EventArgs e)
	{
		MainFab.BackgroundColor = Colors.Red;
		StatusLabel.Text = "Color = Red";
	}

	void OnSetPurple(object? sender, EventArgs e)
	{
		MainFab.BackgroundColor = Color.FromArgb("#6200EE");
		StatusLabel.Text = "Color = Purple";
	}

	void OnSetGreen(object? sender, EventArgs e)
	{
		MainFab.BackgroundColor = Colors.Green;
		StatusLabel.Text = "Color = Green";
	}

	void OnElevation0(object? sender, EventArgs e)
	{
		MainFab.Elevation = 0;
		StatusLabel.Text = "Elevation = 0 (no shadow)";
	}

	void OnElevation12(object? sender, EventArgs e)
	{
		MainFab.Elevation = 12;
		StatusLabel.Text = "Elevation = 12 (deep shadow)";
	}
}