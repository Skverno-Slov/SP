using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabWork17.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows;

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
            _currentDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            LoadDir();
        }

        [RelayCommand]
        void OpenDir()
        {
            if (SelectedItem == null) return;

            try
            {
                if (SelectedItem.IsDir)
                {
                    CurrentDir = SelectedItem.FullPath;
                    LoadDir();
                }
                else
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = SelectedItem.FullPath,
                        UseShellExecute = true 
                    };
                    Process.Start(startInfo);
                }
            }
            catch (Exception)
            {
                MessageBox.Show($"Ошибка при открытии");
            }
        }
        
        [RelayCommand]
        void LoadDir()
        {
            try
            {
                var di = new DirectoryInfo(CurrentDir);
                if (!di.Exists)
                    throw new DirectoryNotFoundException();

                Files.Clear();

                var dirInfo = new DirectoryInfo(CurrentDir);

                if (dirInfo.Parent != null)
                {
                    Files.Add(new FileProperties() { Name = "..", FullPath = dirInfo.Parent.FullName, IsDir = true, Icon = $"{ResFolder}Folder.png" });
                }

                foreach (var dir in dirInfo.GetDirectories())
                {
                    Files.Add(new FileProperties
                    {
                        Name = dir.Name,
                        FullPath = dir.FullName,
                        IsDir = true,
                        LastUpdate = dir.LastWriteTime,
                        Icon = $"{ResFolder}Folder.png"
                    });
                }

                foreach (var file in dirInfo.GetFiles())
                {
                    Files.Add(new FileProperties
                    {
                        Name = file.Name,
                        FullPath = file.FullName,
                        IsDir = false,
                        Extension = file.Extension,
                        LastUpdate = file.LastWriteTime,
                        Size = GetFormatedSize(file.Length),
                        Icon = GetIcon(file.Extension)
                    });
                }
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Католог не найден");
            }
            catch 
            {
                MessageBox.Show("ХЗ");
            }
        }

        [RelayCommand]
        private void Copy(System.Collections.IList? selectedItems)
        {
            if (selectedItems == null || selectedItems.Count == 0) return;

            var fileList = new StringCollection();
            foreach (var item in selectedItems.Cast<FileProperties>())
            {
                fileList.Add(item.FullPath);
            }

            Clipboard.SetFileDropList(fileList);
        }

        [RelayCommand]
        private void Paste()
        {
            if (!Clipboard.ContainsFileDropList()) return;

            StringCollection fileList = Clipboard.GetFileDropList();

            try
            {
                foreach (string sourcePath in fileList)
                {
                    string fileName = Path.GetFileName(sourcePath);
                    string destPath = Path.Combine(CurrentDir, fileName);

                    if (Directory.Exists(sourcePath))
                    {
                        CopyDirectory(sourcePath, destPath);
                    }
                    else
                    {
                        File.Copy(sourcePath, destPath, true);
                    }
                }
                LoadDir();
            }
            catch (Exception)
            {
                MessageBox.Show($"Ошибка вставки");
            }
        }

        [RelayCommand(CanExecute = nameof(CanCompress))]
        private void Compress(FileProperties? item)
        {
            if (item is null) return;
            try
            {
                string zipPath = $"{item.FullPath}.zip";
                if (File.Exists(zipPath)) File.Delete(zipPath);
                ZipFile.CreateFromDirectory(item.FullPath, zipPath);
                LoadDir();
            }
            catch (Exception)
            {
                MessageBox.Show("Не успешно"); 
            }
        }

        [RelayCommand(CanExecute = nameof(CanDecompress))]
        private void Decompress(FileProperties? item)
        {
            if (item is null) return;
            try
            {
                string extractPath = Path.Combine(Path.GetDirectoryName(item.FullPath)!, Path.GetFileNameWithoutExtension(item.FullPath));

                if (!Directory.Exists(extractPath))
                    Directory.CreateDirectory(extractPath);

                ZipFile.ExtractToDirectory(item.FullPath, extractPath);
                LoadDir();
            }
            
            catch (Exception)
            {
                MessageBox.Show("Не успешно");
            }
        }

        private bool CanDecompress(FileProperties? item)
            => item != null && !item.IsDir && Path.GetExtension(item.FullPath).ToLower() == ".zip";

        private bool CanCompress(FileProperties? item)
            => item != null && item.IsDir && item.Name != "..";

        private void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);
            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(destDir, Path.GetFileName(file)), true);
            foreach (var directory in Directory.GetDirectories(sourceDir))
                CopyDirectory(directory, Path.Combine(destDir, Path.GetFileName(directory)));
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
