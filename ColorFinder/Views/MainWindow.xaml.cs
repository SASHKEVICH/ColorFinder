using System.Windows;

namespace ColorFinder.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MinimizeWindowOnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        
        private void MaximizeWindowOnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }
        
        private void CloseWindowOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}