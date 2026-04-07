using System.Drawing;

internal class Program
{
    private static void Main(string[] args)
    {
        int count = 0;
        object locker = new object();

        string ImagesInputDir = Path.Combine(Directory.GetCurrentDirectory(), "Images");
        string ImagesOutputDir = Path.Combine(Directory.GetCurrentDirectory(), "ImagesOut");

        if (!Directory.Exists(ImagesOutputDir))
        {
            Directory.CreateDirectory(ImagesOutputDir);
        }

        string[] files = Directory.GetFiles(ImagesInputDir);

        int total = files.Length;

        Parallel.ForEach(files, file =>
        {

            byte[] bytes = File.ReadAllBytes(file);
            using var ms = new MemoryStream(bytes);
            using var sourceBitmap = new Bitmap(ms);
            using var targetBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height); 

            for (int x = 0; x < targetBitmap.Height; x++)
            {
                for (int y = 0; y < targetBitmap.Width; y++)
                {
                    var oldColor = targetBitmap.GetPixel(x, y);
                    var invertedColor = Color.FromArgb(oldColor.A, 255 - oldColor.R, 255 - oldColor.G, 255 - oldColor.B);

                    targetBitmap.SetPixel(x, y, invertedColor);
                }
            }

            targetBitmap.Save(Path.Combine(ImagesOutputDir, Path.GetFileName(file)));

            int current = Interlocked.Increment(ref count);

            lock (locker)
            {
                int progress = current / total;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.WriteLine($"[{new string('#', progress / 2)}{new string('-', 50 - progress / 2)}]");
            }

        });
    }
}