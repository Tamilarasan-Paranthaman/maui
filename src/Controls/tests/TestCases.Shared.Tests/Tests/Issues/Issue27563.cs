using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
	public class Issue27563 : _IssuesUITest
	{
		public override string Issue => "[Windows] CarouselView Scrolling Issue";

		public Issue27563(TestDevice device)
		: base(device)
		{ }

		[Test]
		[Category(UITestCategories.CarouselView)]
		public void VerifyCarouselViewIndicatorPositionWithoutLooping()
		{
			App.WaitForElement("carouselview");

			App.Tap("ScrollToSecondButton");
			App.WaitForElement("Actual View");

			App.Tap("PositionButton");
			App.WaitForElement("Percentage View");

			App.Tap("PingButton");
			App.WaitForElement("Ping:1");
		}
	}
}
