using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue26878 : _IssuesUITest
{
	public override string Issue => "[Windows] DisplayAlert does not reflect Windows theme changes";

	public Issue26878(TestDevice device)
	: base(device)
	{ }

	[Test]
	[Category(UITestCategories.DisplayAlert)]
	public void VerifyDisplayAlertTheme()
	{
		try
		{
			App.WaitForElement("ShowAlertButton");
			App.SetDarkTheme();
			App.Tap("ShowAlertButton");
			VerifyScreenshot();
		}
		finally
		{
			App.Tap("OK");
			App.SetLightTheme();
		}
	}
}