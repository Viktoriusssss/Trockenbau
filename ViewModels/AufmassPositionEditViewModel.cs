using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModernWPFApp.Models;
using System.ComponentModel;

namespace ModernWPFApp.ViewModels
{
    public partial class AufmassPositionEditViewModel : ObservableObject
    {
        [ObservableProperty] private AufmassPosition _position;
        [ObservableProperty] private string _errorMessage = string.Empty;
        [ObservableProperty] private bool _isValid = false;

        public string WindowTitle => Position.Id == 0 ? "Neue Position hinzufügen" : "Position bearbeiten";
        public event EventHandler? SaveRequested;
        public event EventHandler? CancelRequested;

        public AufmassPositionEditViewModel()
        {
            Position = new AufmassPosition
            {
                Position = "",
                Beschreibung = "",
                Länge = 0,
                Breite = 0,
                Höhe = 0,
                Std = 0,
                Stück = 0,
                Lfm = 0,
                Einzelpreis = 0,
                Gesamt = 0
            };

            Position.PropertyChanged += OnPositionPropertyChanged;
            ValidatePosition();
        }

        public AufmassPositionEditViewModel(AufmassPosition position)
        {
            // Create a copy to avoid modifying the original
            Position = new AufmassPosition
            {
                Id = position.Id,
                AufmassId = position.AufmassId,
                Position = position.Position,
                Beschreibung = position.Beschreibung,
                Länge = position.Länge,
                Breite = position.Breite,
                Höhe = position.Höhe,
                Std = position.Std,
                Stück = position.Stück,
                Lfm = position.Lfm,
                Einzelpreis = position.Einzelpreis,
                Gesamt = position.Gesamt
            };

            Position.PropertyChanged += OnPositionPropertyChanged;
            ValidatePosition();
        }

        private void OnPositionPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ValidatePosition();
        }

        private void ValidatePosition()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Position.Position))
                errors.Add("Position ist erforderlich");

            if (string.IsNullOrWhiteSpace(Position.Beschreibung))
                errors.Add("Beschreibung ist erforderlich");

            if (Position.Einzelpreis < 0)
                errors.Add("Einzelpreis darf nicht negativ sein");

            ErrorMessage = string.Join("\n", errors);
            IsValid = errors.Count == 0;
        }

        [RelayCommand]
        private void Save()
        {
            if (IsValid)
            {
                SaveRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}


