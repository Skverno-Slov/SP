using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;
using LabWork17.Model;

namespace LabWork17.ViewModel
{
    public partial class FileListViewModel : ObservableObject
    {
        const string ResFolder = "/Res/Icons/";

        [ObservableProperty]
        string _currentDir;

        [ObservableProperty]
        FileProperties _selectedItem;

        public ObservableCollection<FileProperties> Files { get; } = new();

        public FileListViewModel()
        {
            _currentDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            LoadDir();
        }

        [RelayCommand]
        void OpenDir()
        {
            if (SelectedItem == null && !SelectedItem.isDir) return;
            
            CurrentDir = SelectedItem.Fullpath;

            LoadDir();
        }

        [RelayCommand]
        void LoadDir()
        {
            //if (Directory.Exists(_currentDir))
            //    return;

            Files.Clear();

            var dirInfo = new DirectoryInfo(CurrentDir);

            if (dirInfo.Parent != null)
            {
                Files.Add(new FileProperties() { Name = "..", Fullpath = dirInfo.Parent.FullName, isDir = true, Icon = $"{ResFolder}Folder.png" });
            }

            foreach (var dir in dirInfo.GetDirectories())
            {
                Files.Add(new FileProperties
                {
                    Name = dir.Name,
                    Fullpath = dir.FullName,
                    isDir = true,
                    LastUpdate = dir.LastWriteTime,
                    Icon = $"{ResFolder}Folder.png"
                });
            }

            foreach (var file in dirInfo.GetFiles())
            {
                Files.Add(new FileProperties
                {
                    Name = file.Name,
                    Fullpath = file.FullName,
                    isDir = false,
                    Extension = file.Extension,
                    LastUpdate = file.LastWriteTime,
                    Size = GetFormatedSize(file.Length),
                    Icon = GetIcon(file.Extension)
                });
            }
        }

        string GetIcon(string extension)
        {
            switch(extension.ToLower())
            {
                case ".jpg" or ".png":
                    return $"{ResFolder}Image.png";
                case ".txt" or ".docx":
                    return $"{ResFolder}TxtFile.png";
                case ".zip":
                    return $"{ResFolder}Zip.png";
                default:
                    return $"{ResFolder}Default.png";
            }
        }

        string GetFormatedSize(long bytes)
        {
            int convertNumber = 1024;

            string[] units = { "Б", "Кб", "Мб", "Гб" };
            double size = bytes;
            int unitIndex = 0;
            while (size >= convertNumber && unitIndex < units.Length - 1)
            {
                size /= convertNumber;
                unitIndex++;
            }
            return $"{Math.Round(size, 4)} {units[unitIndex]}";
        }
    }
}
