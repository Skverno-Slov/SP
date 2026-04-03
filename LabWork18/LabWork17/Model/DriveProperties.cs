using LabWork17.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabWork17.Model
{
    public class DriveProperties
    {
        readonly AppService _service = new();

        public string Name { get; set; } = string.Empty;     
        public string Label { get; set; } = string.Empty;    
        public long TotalBytes { get; set; }
        public long FreeBytes { get; set; }

        public string TotalSizeDisplay => _service.GetFormatedSize(TotalBytes);
        public string FreeSpaceDisplay => _service.GetFormatedSize(FreeBytes);
        public string DisplayName => string.IsNullOrEmpty(Label) ? Name : $"{Label} ({Name})";

        public double UsedPercentage => TotalBytes == 0 ? 0 :
            ((double)(TotalBytes - FreeBytes) / TotalBytes) * 100;
    }
}
