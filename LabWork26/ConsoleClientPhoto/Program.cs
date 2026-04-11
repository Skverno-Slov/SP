using System.Net;
using System.Net.Sockets;

class ImageClient
{
    static void Main()
    {
        Console.WriteLine("Введите путь к файлу: ");
        string filePath = Console.ReadLine();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не найдён, лох!");
            return;
        }
        byte[] imageBytes = File.ReadAllBytes(filePath);
        long fileSize = imageBytes.Length;

        try
        {
            using var client = new TcpClient();
            client.Connect(IPAddress.Loopback, 8890);
            using NetworkStream stream = client.GetStream();

            byte[] sizeBytes = BitConverter.GetBytes(fileSize);
            stream.Write(sizeBytes, 0, sizeBytes.Length);

            stream.Write(imageBytes, 0, imageBytes.Length);

            byte[] responseImageBytes = new byte[8];
            int bytesRead = stream.Read(responseImageBytes, 0, 8);
            long responseSize = BitConverter.ToInt64(responseImageBytes, 0);

            byte[] resizedImageBytes = new byte[responseSize];
            int totalRead = 0;
            while (totalRead < responseSize)
            {
                int read = stream.Read(resizedImageBytes, totalRead, (int)(responseSize - totalRead));
                if (read == 0) break;
                totalRead += read;
            }

            string outputPath = Path.GetFileNameWithoutExtension(filePath) + "_resized.jpg";
            File.WriteAllBytes(outputPath, resizedImageBytes);
            Console.WriteLine("Сохранено изображение");
        }
        catch
        {
            Console.WriteLine("Одна ошибка и ты с аурой креветки(Матвея)");
        }
    }

}