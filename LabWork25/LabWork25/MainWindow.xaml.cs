using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LabWork25
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainGrid.Children.Add(new CustomButton() { Width = 150, Height = 100, Text = "Пример текста" });
            MainGrid.Children.Add(new CustomButton() { Width = 150, Height = 100, Text = "Пример lheujq", TextColor = Brushes.Red, BackgroundColor = Colors.DarkGray });
            MainGrid.Children.Add(new CustomButton() { Width = 150, Height = 100, Text = "ядерка", BackgroundColor = Colors.Red, TextSize = 25 });
        }
    }
}