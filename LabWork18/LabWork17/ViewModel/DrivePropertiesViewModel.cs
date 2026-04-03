using CommunityToolkit.Mvvm.ComponentModel;
using LabWork17.Model;
using LabWork17.Services;
using System.IO;
using System.Xml.Linq;

namespace LabWork17.ViewModel
{
    public partial class DrivePropertiesViewModel : ObservableObject
    {
        readonly AppService _service = new();

        [ObservableProperty] private string _label;
        [ObservableProperty] private string _fileSystem;

        public long UsedBytes { get; }
        public long FreeBytes { get; }
        public long TotalBytes { get; }
        public string Type { get; }

        public string UsedSpace => _service.GetFormatedSize(UsedBytes);
        public string FreeSpace => _service.GetFormatedSize(FreeBytes);
        public string TotalSpace => _service.GetFormatedSize(TotalBytes);

        public DrivePropertiesViewModel(DriveProperties drive)
        {
            var info = new DriveInfo(drive.Name);
            Type = info.DriveType.ToString();
            Label = info.VolumeLabel;
            FileSystem = info.DriveFormat;
            TotalBytes = info.TotalSize;
            FreeBytes = info.AvailableFreeSpace;
            UsedBytes = TotalBytes - FreeBytes;
        }
    }
}
