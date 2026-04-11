using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener listener = new(IPAddress.Any, 8890);
listener.Start();

while (true)
{
    TcpClient client = await listener.AcceptTcpClientAsync();
    _ = Task.Run(() =>
    {
        using (client)
        using (NetworkStream stream = client.GetStream())
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    string datetime = DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy");

                    Console.WriteLine($"{message} ({datetime})");
                }
                catch
                {
                    break;
                }
            }
        }
    });
}