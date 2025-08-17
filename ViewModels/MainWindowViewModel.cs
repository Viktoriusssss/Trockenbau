using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModernWPFApp.Services;
using ModernWPFApp.Models;
using System.Collections.ObjectModel;

namespace ModernWPFApp.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly AuthenticationService _authService;

        [ObservableProperty]
        private User? _currentUser;

        [ObservableProperty]
        private int _selectedTabIndex = 0;

        [ObservableProperty]
        private string _statusMessage = "Bereit";

        public ObservableCollection<TabViewModel> Tabs { get; }

        public MainWindowViewModel()
        {
            _authService = AuthenticationService.Instance;
            CurrentUser = _authService.CurrentUser;

            Tabs = new ObservableCollection<TabViewModel>
            {
                new TabViewModel("Kunden", "AccountMultiple", "Kundenverwaltung"),
                new TabViewModel("Benutzer", "AccountGroup", "Benutzerverwaltung"),
                new TabViewModel("Baustellen", "City", "Baustellenverwaltung"),
                new TabViewModel("Angebote", "FileDocument", "Angebotsverwaltung"),
                new TabViewModel("Aufmaß", "Ruler", "Aufmaßverwaltung"),
                new TabViewModel("Rechnungen", "Receipt", "Rechnungsverwaltung"),
                new TabViewModel("LV", "FormatListNumbered", "Leistungsverzeichnis"),
                new TabViewModel("Über", "Information", "Über die Anwendung")
            };

            _authService.UserLoggedOut += OnUserLoggedOut;
        }

        [RelayCommand]
        private void Logout()
        {
            _authService.Logout();
        }

        [RelayCommand]
        private void ShowUserProfile()
        {
            StatusMessage = $"Profil von {CurrentUser?.FullName} anzeigen";
        }

        [RelayCommand]
        private void ShowSettings()
        {
            if (_authService.HasPermission("SystemSettings"))
            {
                StatusMessage = "Einstellungen öffnen";
            }
            else
            {
                StatusMessage = "Keine Berechtigung für Systemeinstellungen";
            }
        }

        [RelayCommand]
        private void RefreshData()
        {
            StatusMessage = "Daten werden aktualisiert...";
            // TODO: Implement refresh logic
            StatusMessage = "Daten aktualisiert";
        }

        [RelayCommand]
        private void ShowHelp()
        {
            StatusMessage = "Hilfe öffnen";
        }

        private void OnUserLoggedOut(object? sender, EventArgs e)
        {
            // This will be handled by the App.xaml.cs to show login window again
        }

        public void SetStatusMessage(string message)
        {
            StatusMessage = message;
        }

        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            
            if (e.PropertyName == nameof(SelectedTabIndex))
            {
                StatusMessage = $"Aktuell: {Tabs[SelectedTabIndex].Title}";
                // Trigger refresh when tab changes
                OnTabChanged();
            }
        }

        private void OnTabChanged()
        {
            // This will be called when tab changes
            // The individual ViewModels will handle their own refresh
        }
    }

    public class TabViewModel
    {
        public string Title { get; set; }
        public string IconKind { get; set; }
        public string Description { get; set; }

        public TabViewModel(string title, string iconKind, string description)
        {
            Title = title;
            IconKind = iconKind;
            Description = description;
        }
    }
}
