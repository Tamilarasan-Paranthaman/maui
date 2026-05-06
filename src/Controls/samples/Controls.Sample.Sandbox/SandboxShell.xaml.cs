namespace Maui.Controls.Sample;


public partial class SandboxShell : Shell
{
	public MenuBarViewModel ViewModel { get; }

	public SandboxShell()
	{
		InitializeComponent();
		
		ViewModel = new MenuBarViewModel();
		BindingContext = ViewModel;
		
		// Populate location submenu dynamically
		//PopulateLocationSubMenu();
	}

	private void PopulateLocationSubMenu()
	{
		ChangeLocationSubItem.Clear();
		
		foreach (var location in ViewModel.LocationItems)
		{
			var menuItem = new MenuFlyoutItem
			{
				Text = location,
				IsEnabled = false,
				Command = new Command(() => OnLocationSelected(location))
			};
			//ChangeLocationSubItem.Add(menuItem);
		}
	}

	private async void OnLocationSelected(string location)
	{
		if (Application.Current?.Windows[0].Page != null)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			await Application.Current.Windows[0].Page.DisplayAlertAsync("Location Changed", $"Selected: {location}", "OK");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
	}

	private void OnToggleFileMenuClicked(object sender, EventArgs e)
	{
		locationsMenuBarItem.IsEnabled = !locationsMenuBarItem.IsEnabled;
	}

	private void OnToggleLocationsMenuClicked(object sender, EventArgs e)
	{
		ViewModel.LocationsMenuEnabled = !ViewModel.LocationsMenuEnabled;
	}

	private void OnToggleViewMenuClicked(object sender, EventArgs e)
	{
		ViewModel.ViewMenuEnabled = !ViewModel.ViewMenuEnabled;
	}

	private void OnToggleMediaMenuClicked(object sender, EventArgs e)
	{
		ViewModel.MediaMenuEnabled = !ViewModel.MediaMenuEnabled;
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		mediaMenuBarItem.IsEnabled = !mediaMenuBarItem.IsEnabled;
	}
}
