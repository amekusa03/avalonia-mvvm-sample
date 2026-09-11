using Avalonia.Controls;

namespace SampleApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // Set the ViewModel handling UI logic as the DataContext (data source) for this window.
        // This allows access to ViewModel properties and commands from XAML.
        DataContext = new MainWindowViewModel();
    }
}
