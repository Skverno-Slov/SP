namespace LabWork17.Services
{
    public class AppService
    {
        public string GetFormatedSize(long bytes)
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
