
namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	public void Button_ClickedAsync(object sender, EventArgs e)
	{
		DisplayAlertAsync("Alert", "You have been alerted", "OK");

		//DisplayActionSheetAsync("Action Sheet", "Cancel", "Option 1", "Option 2", "Option 3");

		//DisplayPromptAsync("Prompt", "Please enter a value", "Cancel", "OK", "Placeholder", -1, Keyboard.Default);
	}

	public async void Button_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new NewPage());
	}

	private void Button_ClickedActionSheet(object sender, EventArgs e)
	{
		DisplayActionSheetAsync("Action Sheet", "Cancel", "Option 1", "Option 2", "Option 3");
	}

	private void Button_ClickedPrompt(object sender, EventArgs e)
	{
		DisplayPromptAsync("Prompt", "Please enter a value", "Cancel", "OK", "Placeholder", -1, Keyboard.Default);
	}
}

class NewPage : ContentPage
{
	public NewPage()
	{
		IsBusy = true;
		var button = new Button()
		{
			AutomationId = "button1",
			Margin = new Thickness(0, 10),
			VerticalOptions = LayoutOptions.Start,
			Text = "Navigate to page 1",
			HeightRequest = 70,
			Command = new Command(() =>
			{
				Navigation.PopAsync();
			})
		};
		var button2 = new Button()
		{
			AutomationId = "button2",
			Margin = new Thickness(0, 10),
			VerticalOptions = LayoutOptions.Start,
			Text = "Set IsBusy to false",
			HeightRequest = 70,
		};
		button2.Clicked += (s, e) =>
		{
			IsBusy = !IsBusy;
			button2.Text = IsBusy ? "Set IsBusy to false" : "Set IsBusy to true";
		};
		Content = new VerticalStackLayout
		{
			Children =
			{
				button,
				button2
			}
		};
	}
}