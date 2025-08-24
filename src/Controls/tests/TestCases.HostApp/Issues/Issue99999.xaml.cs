using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample.Issues
{
    [Issue(IssueTracker.Github, 99999,
        "[Windows] Label disappears on the MainPage after a theme change on another page",
        PlatformAffected.Windows)]
    public partial class Issue99999 : TestContentPage
    {
        private bool _isTheme1 = true;
        private ResourceDictionary _theme1;
        private ResourceDictionary _theme2;

        public Issue99999()
        {
            InitializeComponent();
        }

        protected override void Init()
        {
            // Get references to the theme dictionaries from the page resources
            _theme1 = (ResourceDictionary)Resources["Theme1"];
            _theme2 = (ResourceDictionary)Resources["Theme2"];

            // Apply initial theme
            ApplyTheme(_theme1);
        }

        void OnToggleThemeClicked(object sender, EventArgs e)
        {
            // Simulate the theme switching behavior described in the issue
            _isTheme1 = !_isTheme1;
            
            // Clear and re-add merged dictionaries - this is what causes the issue
            Application.Current.Resources.MergedDictionaries.Clear();
            
            if (_isTheme1)
            {
                Application.Current.Resources.MergedDictionaries.Add(_theme1);
                StatusLabel.Text = "Test Status: Theme 1 Applied";
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(_theme2);
                StatusLabel.Text = "Test Status: Theme 2 Applied";
            }
        }

        private void ApplyTheme(ResourceDictionary theme)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(theme);
        }
    }
}