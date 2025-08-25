using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues;

public class Issue31326 : _IssuesUITest
{
	public override string Issue => "[iOS] Clear button does not apply TextColor when set at runtime";

#if IOS
	private const int CropBottomValue = 1550;
#elif ANDROID
	private const int CropBottomValue = 1150;
#elif WINDOWS
	private const int CropBottomValue = 400;
#else
	private const int CropBottomValue = 360;
#endif

	public Issue31326(TestDevice device)
	: base(device)
	{ }

	[Test]
	[Category(UITestCategories.Entry)]
	public void VerifyEntryClearButtonColor()
	{
		App.WaitForElement("SetTextColorButton");
		App.Tap("SetTextColorButton");
		App.Tap("SampleEntry");
		VerifyScreenshot(cropBottom: CropBottomValue);
	}
}