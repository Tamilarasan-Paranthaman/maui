namespace Maui.Controls.Sample.Issues;

[Issue(IssueTracker.Github, 31326, "[iOS] Clear button does not apply TextColor when set at runtime", PlatformAffected.iOS)]
public class Issue31326 : ContentPage
{
	public Issue31326()
	{
		var entry = new UITestEntry
		{
			Text = "Sample Entry",
			IsCursorVisible = false,
			ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
			AutomationId = "SampleEntry"
		};

		var button = new Button
		{
			Text = "Set Text Color to Red",
			AutomationId = "SetTextColorButton"
		};

		button.Clicked += (sender, args) =>
		{
			entry.TextColor = Colors.Red;
		};

		var verticalStackLayout = new VerticalStackLayout
		{
			Spacing = 30,
			Padding = 20,
			Children =
			{
				entry,
				button
			},
		};
		Content = verticalStackLayout;
	}
}