using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using WGridLength = Microsoft.UI.Xaml.GridLength;
using WThickness = Microsoft.UI.Xaml.Thickness;

namespace Microsoft.Maui.Platform
{
	public partial class MauiFloatingActionButton : Button
	{
		readonly Image _image;
		readonly TextBlock _text;
		readonly Grid _contentGrid;

		public MauiFloatingActionButton()
		{
			_image = new Image
			{
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center,
				Stretch = Stretch.Uniform,
			};

			_text = new TextBlock
			{
				VerticalAlignment = VerticalAlignment.Center,
				Visibility = UI.Xaml.Visibility.Collapsed,
			};

			_contentGrid = new Grid
			{
				ColumnSpacing = 8,
			};

			_contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = WGridLength.Auto });
			_contentGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = WGridLength.Auto });
			AddChild(_contentGrid, _image);
			AddChild(_contentGrid, _text);
			Grid.SetColumn(_text, 1);

			Content = _contentGrid;
			HorizontalContentAlignment = HorizontalAlignment.Center;
			VerticalContentAlignment = VerticalAlignment.Center;
			Padding = new WThickness(0);
		}

		/// <summary>
		/// Gets the image element used to display the FAB icon.
		/// </summary>
		internal Image IconImage => _image;

		/// <summary>
		/// Gets the text block element used to display the FAB label.
		/// </summary>
		internal TextBlock Label => _text;

		[SuppressMessage("ApiDesign", "RS0030:Do not use banned APIs", Justification = "Grid is used as button content, not as a layout panel.")]
		static void AddChild(Grid grid, UIElement child) => grid.Children.Add(child);
	}
}
