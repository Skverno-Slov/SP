using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LabWork17.Model;
using LabWork17.View;
using LabWork17.View.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LabWork17.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private object? _currentPage;

        [ObservableProperty]
        private string _currentPath = "";

        public MainViewModel()
        {
            GoToMyComputer(); 

            WeakReferenceMessenger.Default.Register<NavigateToPathMessage>(this, (r, m) => {
                CurrentPath = m.Value;
                GoToExplorer();
            });
        }

        [RelayCommand]
        public void GoToMyComputer() 
        {
            CurrentPath = "";
            CurrentPage = new DrivesPage();
        }

        [RelayCommand]
        public void GoToExplorer()
        {
            try
            {
                if (String.IsNullOrWhiteSpace(CurrentPath))
                    GoToMyComputer();

                var di = new DirectoryInfo(CurrentPath);
                if (!di.Exists)
                    throw new DirectoryNotFoundException();

                CurrentPage = new FileListPage(CurrentPath);
            }
            catch 
            {
                MessageBox.Show("Католог не найден");
            }
        }

        partial void OnCurrentPathChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                GoToMyComputer();
        }
    }
}
