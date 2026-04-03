using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LabWork17.Model;
using LabWork17.View;
using System.Collections.ObjectModel;
using System.IO;

namespace LabWork17.ViewModel
{
    public partial class DrivesViewModel : ObservableObject
    {
        [ObservableProperty]
        private DriveProperties? _selectedDrive;

        public ObservableCollection<DriveProperties> Drives { get; } = new();

        public DrivesViewModel() => LoadDrives();

        private void LoadDrives()
        {
            Drives.Clear();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady) 
                {
                    Drives.Add(new DriveProperties
                    {
                        Name = drive.Name,
                        Label = drive.VolumeLabel,
                        TotalBytes = drive.TotalSize,
                        FreeBytes = drive.TotalFreeSpace
                    });
                }
            }
        }

        [RelayCommand]
        private void ShowProperties(DriveProperties? drive)
        {
            if (drive is null) return;

            var window = new DrivePropertiesWindow(drive);
            window.ShowDialog();
        }

        [RelayCommand]
        private void OpenDrive() 
        {
            if (SelectedDrive is null) return;

            WeakReferenceMessenger.Default.Send(new NavigateToPathMessage(SelectedDrive.Name));
        }
    }
}
