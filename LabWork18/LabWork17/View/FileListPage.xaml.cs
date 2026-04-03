using LabWork17.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LabWork17.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для FileListPage.xaml
    /// </summary>
    public partial class FileListPage : Page
    {
        public FileListPage(string dir)
        {
            InitializeComponent();
            DataContext = new FileListViewModel(dir);
        }
    }
}
