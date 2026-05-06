using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maui.Controls.Sample;

public partial class MainPage : ContentPage
{
    private MenuBarViewModel ViewModel => ((SandboxShell)Application.Current!.Windows[0].Page!).ViewModel;

    public MainPage()
    {
        InitializeComponent();
    }

    // MenuBarItem Level Handlers
    private void OnFileMenuToggled(object? sender, ToggledEventArgs e)
    {
        ViewModel.FileMenuEnabled = e.Value;
    }

    private void OnLocationsMenuToggled(object? sender, ToggledEventArgs e)
    {
        ViewModel.LocationsMenuEnabled = e.Value;
    }

    private void OnChangeLocationSubItemToggled(object? sender, ToggledEventArgs e)
    {
        ViewModel.ChangeLocationEnabled = e.Value;
    }

    private void OnRemoveLocationItemToggled(object? sender, ToggledEventArgs e)
    {
        ViewModel.RemoveLocationEnabled = e.Value;
    }

    private void OnViewMenuToggled(object? sender, ToggledEventArgs e)
    {
        ViewModel.ViewMenuEnabled = e.Value;
    }

    private void OnMediaMenuToggled(object? sender, ToggledEventArgs e)
    {
        ViewModel.MediaMenuEnabled = e.Value;
    }

    private void OnExitItemToggled(object? sender, ToggledEventArgs e)
    {
        ViewModel.ExitEnabled = e.Value;
    }

    private void OnAddLocationItemToggled(object? sender, ToggledEventArgs e)
    {
        ViewModel.AddLocationEnabled = e.Value;
    }

    private void OnEditLocationItemToggled(object? sender, ToggledEventArgs e)
    {
        ViewModel.EditLocationEnabled = e.Value;
    }

    private void OnRefreshItemToggled(object? sender, ToggledEventArgs e)
    {
        ViewModel.RefreshEnabled = e.Value;
    }

    private void OnPlayItemToggled(object? sender, ToggledEventArgs e)
    {
        ViewModel.PlayEnabled = e.Value;
    }

    private void OnPauseItemToggled(object? sender, ToggledEventArgs e)
    {
        ViewModel.PauseEnabled = e.Value;
    }

    private void OnStopItemToggled(object? sender, ToggledEventArgs e)
    {
        ViewModel.StopEnabled = e.Value;
    }
}

public class MenuBarViewModel : INotifyPropertyChanged
{
    // File Menu Properties
    private string _fileMenuText = "File";
    private bool _fileMenuEnabled = true;
    private string _exitText = "Exit";
    private bool _exitEnabled = true;
    private string _exitIcon = "exit.png";

    // Locations Menu Properties
    private string _locationsMenuText = "Locations";
    private bool _locationsMenuEnabled = true;
    private string _changeLocationText = "Change Location";
    private bool _changeLocationEnabled = false;
    private string _addLocationText = "Add Location";
    private bool _addLocationEnabled = false;
    private string _editLocationText = "Edit Location";
    private bool _editLocationEnabled = true;
    private string _removeLocationText = "Remove Location";
    private bool _removeLocationEnabled = false;

    // View Menu Properties
    private string _viewMenuText = "View";
    private bool _viewMenuEnabled = true;
    private string _refreshText = "Refresh";
    private string _refreshIcon = "refresh.png";
    private bool _refreshEnabled = false;

    // Media Menu Properties
    private bool _mediaMenuEnabled = false;
    private bool _playEnabled = false;
    private bool _pauseEnabled = false;
    private bool _stopEnabled = false;

    public MenuBarViewModel()
    {
        // Initialize commands
        ExitCommand = new Command(OnExit);
        AddLocationCommand = new Command(OnAddLocation);
        EditLocationCommand = new Command(OnEditLocation);
        RemoveLocationCommand = new Command(OnRemoveLocation);
        RefreshCommand = new Command(OnRefresh);

        // Initialize location items
        LocationItems = new ObservableCollection<string>
            {
                "New York",
                "London",
                "Tokyo",
                "Paris",
                "Sydney"
            };
    }

    // File Menu Properties
    public string FileMenuText
    {
        get => _fileMenuText;
        set { _fileMenuText = value; OnPropertyChanged(); }
    }

    public bool FileMenuEnabled
    {
        get => _fileMenuEnabled;
        set { _fileMenuEnabled = value; OnPropertyChanged(); }
    }

    public string ExitText
    {
        get => _exitText;
        set { _exitText = value; OnPropertyChanged(); }
    }

    public bool ExitEnabled
    {
        get => _exitEnabled;
        set
        {
            _exitEnabled = value;
            OnPropertyChanged();
        }
    }

    public string ExitIcon
    {
        get => _exitIcon;
        set { _exitIcon = value; OnPropertyChanged(); }
    }

    // Locations Menu Properties
    public string LocationsMenuText
    {
        get => _locationsMenuText;
        set { _locationsMenuText = value; OnPropertyChanged(); }
    }

    public bool LocationsMenuEnabled
    {
        get => _locationsMenuEnabled;
        set { _locationsMenuEnabled = value; OnPropertyChanged(); }
    }

    public string ChangeLocationText
    {
        get => _changeLocationText;
        set { _changeLocationText = value; OnPropertyChanged(); }
    }

    public bool ChangeLocationEnabled
    {
        get => _changeLocationEnabled;
        set { _changeLocationEnabled = value; OnPropertyChanged(); }
    }

    public string AddLocationText
    {
        get => _addLocationText;
        set { _addLocationText = value; OnPropertyChanged(); }
    }

    public bool AddLocationEnabled
    {
        get => _addLocationEnabled;
        set
        {
            _addLocationEnabled = value;
            OnPropertyChanged();
        }
    }

    public string EditLocationText
    {
        get => _editLocationText;
        set { _editLocationText = value; OnPropertyChanged(); }
    }

    public bool EditLocationEnabled
    {
        get => _editLocationEnabled;
        set
        {
            _editLocationEnabled = value;
            OnPropertyChanged();
        }
    }

    public string RemoveLocationText
    {
        get => _removeLocationText;
        set { _removeLocationText = value; OnPropertyChanged(); }
    }

    public bool RemoveLocationEnabled
    {
        get => _removeLocationEnabled;
        set
        {
            _removeLocationEnabled = value;
            OnPropertyChanged();
        }
    }

    // View Menu Properties
    public string ViewMenuText
    {
        get => _viewMenuText;
        set { _viewMenuText = value; OnPropertyChanged(); }
    }

    public bool ViewMenuEnabled
    {
        get => _viewMenuEnabled;
        set { _viewMenuEnabled = value; OnPropertyChanged(); }
    }

    public string RefreshText
    {
        get => _refreshText;
        set { _refreshText = value; OnPropertyChanged(); }
    }

    public string RefreshIcon
    {
        get => _refreshIcon;
        set { _refreshIcon = value; OnPropertyChanged(); }
    }

    public bool RefreshEnabled
    {
        get => _refreshEnabled;
        set { _refreshEnabled = value; OnPropertyChanged(); }
    }

    // Media Menu Properties
    public bool MediaMenuEnabled
    {
        get => _mediaMenuEnabled;
        set { _mediaMenuEnabled = value; OnPropertyChanged(); }
    }

    public bool PlayEnabled
    {
        get => _playEnabled;
        set { _playEnabled = value; OnPropertyChanged(); }
    }

    public bool PauseEnabled
    {
        get => _pauseEnabled;
        set { _pauseEnabled = value; OnPropertyChanged(); }
    }

    public bool StopEnabled
    {
        get => _stopEnabled;
        set { _stopEnabled = value; OnPropertyChanged(); }
    }

    // Collections
    public ObservableCollection<string> LocationItems { get; set; }

    // Commands
    public ICommand ExitCommand { get; }
    public ICommand AddLocationCommand { get; }
    public ICommand EditLocationCommand { get; }
    public ICommand RemoveLocationCommand { get; }
    public ICommand RefreshCommand { get; }

    // Command Implementations
    private void OnExit()
    {
        Application.Current?.Quit();
    }

    private async void OnAddLocation()
    {
        if (Application.Current?.Windows[0].Page != null)
            await Application.Current.Windows[0].Page!.DisplayAlertAsync("Add Location", "Add Location clicked", "OK");
    }

    private async void OnEditLocation()
    {
        if (Application.Current?.Windows[0].Page != null)
            await Application.Current.Windows[0].Page!.DisplayAlertAsync("Edit Location", "Edit Location clicked", "OK");
    }

    private async void OnRemoveLocation()
    {
        if (Application.Current?.Windows[0].Page != null)
            await Application.Current.Windows[0].Page!.DisplayAlertAsync("Remove Location", "Remove Location clicked", "OK");
    }

    private async void OnRefresh()
    {
        if (Application.Current?.Windows[0].Page != null)
            await Application.Current.Windows[0].Page!.DisplayAlertAsync("Refresh", "Refresh clicked", "OK");
    }

    // INotifyPropertyChanged Implementation
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}