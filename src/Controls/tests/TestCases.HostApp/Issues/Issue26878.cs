namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 26878, "[Windows] DisplayAlert does not reflect Windows theme changes", PlatformAffected.UWP)]
public class Issue26878 : ContentPage
{

	public Issue26878()
	{
		var button = new Button
		{
			Text = "Show Alert",
			AutomationId = "ShowAlertButton",
			VerticalOptions = LayoutOptions.Center,
			HorizontalOptions = LayoutOptions.Center
		};

		button.Clicked += (sender, e) =>
		{
			DisplayAlert("Welcome to MAUI!", "You clicked the alert button!", "OK");
		};

		var verticalStackLayout = new VerticalStackLayout()
		{
			Spacing = 20,
			Padding = new Thickness(20),
		};

		verticalStackLayout.Add(button);

		Content = verticalStackLayout;
	}
}
