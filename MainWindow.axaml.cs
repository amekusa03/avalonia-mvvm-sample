using Avalonia.Controls;

namespace SampleApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // UIのロジックを担うViewModelを、このウィンドウのDataContext（データ源）として設定します。
        // これにより、XAML側からViewModelのプロパティやコマンドにアクセスできるようになります。
        DataContext = new MainWindowViewModel();
    }
}