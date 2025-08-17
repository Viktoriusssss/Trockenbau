using System.Windows.Controls;
using System.Windows.Input;
using ModernWPFApp.ViewModels;

namespace ModernWPFApp.Views
{
    public partial class BenutzerView : UserControl
    {
        public BenutzerView()
        {
            InitializeComponent();
            DataContext = new BenutzerViewModel();
        }

        private void BenutzerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This event handler is called when the selection changes in the DataGrid
            // The binding will handle the actual selection logic
        }

        private void DataGridRow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // This event handler is called when a row is clicked
            // The binding will handle the actual selection logic
        }
    }
}
