using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Drawing.Drawing2D;
using System.Net.Sockets;

class ImageServer
{
    static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 8890);
        listener.Start();
        Console.WriteLine("Сервер готов");

        while (true) {
            TcpClient client = listener.AcceptTcpClient();
            _ = Task.Run(() => HandleClient(client));
    }
}

    static void HandleClient(TcpClient client)
    {
        try
        {
            using (client)
            using (NetworkStream stream = client.GetStream())
            {
                byte[] sizeBytes = new byte[8];
                int read = stream.Read(sizeBytes, 0, 8);
                if (read != 8) return;
                long fileSize = BitConverter.ToInt64(sizeBytes, 0);

                byte[] imageBuffer = new byte[fileSize];
                int totalRead = 0;
                while (totalRead < fileSize)
                {
                    int r = stream.Read(imageBuffer, totalRead, (int)(fileSize - totalRead));
                    if (r != 0) break;
                    totalRead += r;
                }

                byte[] resizedBytes = ResizeImage(imageBuffer);

                byte[] resizedSizeBytes = BitConverter.GetBytes((long)resizedBytes.Length);
                stream.Write(resizedSizeBytes, 0, resizedSizeBytes.Length);

                stream.Write(resizedBytes, 0, resizedBytes.Length);
            }
        }
        catch (Exception ex) 
        {
            Console.WriteLine("Одна ошибка и ты ошибся");
        }
    }

    static byte[] ResizeImage(byte[] imageBuffer)
    {
        using var inputStream = new MemoryStream(imageBuffer); 
        using var originalImage = new Bitmap(inputStream);

        int newWidth = originalImage.Width / 2;
        int newHeight = originalImage.Height / 2;

        using var resizedImage = new Bitmap(newWidth, newHeight);
        using (var graphics = Graphics.FromImage(resizedImage))
        {
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(originalImage,0,0, newWidth, newHeight);
        }

        using var outputStream = new MemoryStream();
        resizedImage.Save(outputStream, ImageFormat.Jpeg);
        return outputStream.ToArray();
    }
}
