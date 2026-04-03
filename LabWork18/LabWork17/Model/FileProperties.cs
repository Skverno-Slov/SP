namespace LabWork17.Model
{
    public class FileProperties
    {
        public string Name { get; set; } = null!;
        public string FullPath { get; set; } = null!;
        public bool IsDir { get; set; }
        public string? Extension { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string Size { get; set; } = null!;
        public string? Icon { get; set; }
    }
}
