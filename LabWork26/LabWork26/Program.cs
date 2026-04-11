using System.Net.Sockets;
using System.Text;

try
{
    using TcpClient client = new TcpClient("127.0.0.1", 8890);
    using NetworkStream stream = client.GetStream();

    byte[] buffer = new byte[256];
    while (true)
    {
        int bytes = stream.Read(buffer, 0, buffer.Length);
        if (bytes == 0) break;

        string response = Encoding.UTF8.GetString(buffer, 0, bytes);
        Console.WriteLine(response);
    }
}
catch
{
    Console.WriteLine("Ужас");
}
