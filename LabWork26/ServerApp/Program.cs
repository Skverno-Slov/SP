using System.Net;
using System.Net.Sockets;
using System.Text;

TcpListener server = new TcpListener(IPAddress.Any, 8890);
server.Start();
Console.WriteLine($"Сервер запущен на {server.LocalEndpoint}");

using TcpClient client = server.AcceptTcpClient();
using NetworkStream stream = client.GetStream();

while (true)
{
    string message = $"Точное время: {DateTime.Now}";
    byte[] data = Encoding.UTF8.GetBytes(message);
    stream.Write(data, 0, data.Length);

    Thread.Sleep(5000);
}
