namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnAboutButtonClicked(object sender, EventArgs e)
	{
		Navigation.PushAsync(new DetailsPage());
	}
}

public class DetailsPage : ContentPage
{
	public DetailsPage()
	{
		Title = "About .NET MAUI";
		BackgroundColor = Colors.White;

		var scrollView = new ScrollView();
		var verticalStackLayout = new VerticalStackLayout
		{
			Padding = new Thickness(20),
			Spacing = 20
		};

		// Main heading
		var headingLabel = new Label
		{
			Text = "About .NET MAUI",
			FontSize = 24,
			FontAttributes = FontAttributes.Bold,
			TextColor = Colors.DarkBlue,
			HorizontalOptions = LayoutOptions.Center,
			Margin = new Thickness(0, 0, 0, 10)
		};

		// Main description
		var descriptionLabel = new Label
		{
			Text = ".NET MAUI (Multi-platform App UI) is a modern, cross-platform framework from Microsoft for building native applications that run on Android, iOS, macOS, and Windows — all from a single shared C# and XAML codebase.",
			FontSize = 16,
			TextColor = Colors.Black,
			LineHeight = 1.4
		};

		// Evolution section
		var evolutionLabel = new Label
		{
			Text = "It's the evolution of Xamarin.Forms, designed to provide better performance, simpler architecture, and deeper native integration.",
			FontSize = 16,
			TextColor = Colors.Black,
			LineHeight = 1.4
		};

		// Key Features heading
		var featuresHeading = new Label
		{
			Text = "Key Features:",
			FontSize = 20,
			FontAttributes = FontAttributes.Bold,
			TextColor = Colors.DarkGreen,
			Margin = new Thickness(0, 10, 0, 5)
		};

		// Features list
		var feature1 = new Label
		{
			Text = "• Single Project System - One project targets all platforms",
			FontSize = 14,
			TextColor = Colors.Black,
			Margin = new Thickness(10, 0, 0, 5)
		};

		var feature2 = new Label
		{
			Text = "• Handler Architecture - Better performance than renderers",
			FontSize = 14,
			TextColor = Colors.Black,
			Margin = new Thickness(10, 0, 0, 5)
		};

		var feature3 = new Label
		{
			Text = "• Hot Reload - Real-time XAML and C# code changes",
			FontSize = 14,
			TextColor = Colors.Black,
			Margin = new Thickness(10, 0, 0, 5)
		};

		var feature4 = new Label
		{
			Text = "• Native Performance - Direct access to platform APIs",
			FontSize = 14,
			TextColor = Colors.Black,
			Margin = new Thickness(10, 0, 0, 5)
		};

		var feature5 = new Label
		{
			Text = "• Modern .NET - Built on .NET 6+ with latest C# features",
			FontSize = 14,
			TextColor = Colors.Black,
			Margin = new Thickness(10, 0, 0, 5)
		};

		// Platform support
		var platformsHeading = new Label
		{
			Text = "Supported Platforms:",
			FontSize = 20,
			FontAttributes = FontAttributes.Bold,
			TextColor = Colors.DarkRed,
			Margin = new Thickness(0, 10, 0, 5)
		};

		var platforms = new Label
		{
			Text = "✓ Android (API 21+)\n✓ iOS (11.0+)\n✓ macOS (10.15+)\n✓ Windows (Windows 10 version 1809+)",
			FontSize = 16,
			TextColor = Colors.Black,
			LineHeight = 1.6,
			Margin = new Thickness(10, 0, 0, 0)
		};

		// Add all elements to stack layout
		verticalStackLayout.Children.Add(headingLabel);
		verticalStackLayout.Children.Add(descriptionLabel);
		verticalStackLayout.Children.Add(evolutionLabel);
		verticalStackLayout.Children.Add(featuresHeading);
		verticalStackLayout.Children.Add(feature1);
		verticalStackLayout.Children.Add(feature2);
		verticalStackLayout.Children.Add(feature3);
		verticalStackLayout.Children.Add(feature4);
		verticalStackLayout.Children.Add(feature5);
		verticalStackLayout.Children.Add(platformsHeading);
		verticalStackLayout.Children.Add(platforms);

		scrollView.Content = verticalStackLayout;
		Content = scrollView;
	}
}