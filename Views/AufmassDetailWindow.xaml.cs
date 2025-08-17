using ModernWPFApp.Models;
using ModernWPFApp.ViewModels;
using System.Windows;

namespace ModernWPFApp.Views
{
    public partial class AufmassDetailWindow : Window
    {
        public AufmassDetailWindow(Aufmass aufmass)
        {
            InitializeComponent();
            DataContext = new AufmassDetailViewModel(aufmass);
        }
    }
}


