using NUnit.Framework;
using UITest.Appium;
using UITest.Core;

namespace Microsoft.Maui.TestCases.Tests.Issues
{
    public class Issue99999 : _IssuesUITest
    {
        const string TestLabel = "TestLabel";
        const string TestLabel2 = "TestLabel2";
        const string ToggleThemeButton = "ToggleThemeButton";
        const string StatusLabel = "StatusLabel";

        public Issue99999(TestDevice testDevice) : base(testDevice)
        {
        }

        public override string Issue => "[Windows] Label disappears on the MainPage after a theme change on another page";

        [Test]
        [Category(UITestCategories.Label)]
        [Category(UITestCategories.Windows)]
        public void LabelShouldNotDisappearAfterThemeChange()
        {
            // Verify initial state - labels should be visible
            App.WaitForElement(TestLabel);
            App.WaitForElement(TestLabel2);
            App.WaitForElement(ToggleThemeButton);
            
            var initialText1 = App.FindElement(TestLabel).GetText();
            var initialText2 = App.FindElement(TestLabel2).GetText();
            
            Assert.That(initialText1, Does.Contain("should NOT disappear"));
            Assert.That(initialText2, Does.Contain("Another test label"));

            // Toggle the theme - this should trigger the issue described
            App.Tap(ToggleThemeButton);
            
            // Give the UI time to process the theme change
            App.WaitForElement(StatusLabel);
            
            // Verify that labels still exist and are visible after theme change
            App.WaitForElement(TestLabel);
            App.WaitForElement(TestLabel2);
            
            var afterThemeText1 = App.FindElement(TestLabel).GetText();
            var afterThemeText2 = App.FindElement(TestLabel2).GetText();
            
            // Text should be the same - the labels should not have disappeared
            Assert.That(afterThemeText1, Is.EqualTo(initialText1));
            Assert.That(afterThemeText2, Is.EqualTo(initialText2));
            
            // Toggle theme again to test multiple theme changes
            App.Tap(ToggleThemeButton);
            
            // Give the UI time to process the theme change
            App.WaitForElement(StatusLabel);
            
            // Verify labels still exist after second theme change
            App.WaitForElement(TestLabel);
            App.WaitForElement(TestLabel2);
            
            var secondToggleText1 = App.FindElement(TestLabel).GetText();
            var secondToggleText2 = App.FindElement(TestLabel2).GetText();
            
            // Text should still be the same - labels should remain visible
            Assert.That(secondToggleText1, Is.EqualTo(initialText1));
            Assert.That(secondToggleText2, Is.EqualTo(initialText2));
        }
    }
}