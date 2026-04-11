using System.Net.Sockets;
using System.Text;

try
{
    using TcpClient client = new("127.0.0.1", 8890);
    using NetworkStream stream = client.GetStream();

    Console.Write("имя пользователя: ");
    var name = Console.ReadLine();

    while (true)
    {
        Console.Write("> ");
        var message = Console.ReadLine();

        var formatedMessage = $"{name}: {message}";

        byte[] data = Encoding.UTF8.GetBytes(formatedMessage);

        stream.Write(data, 0, data.Length);
    }
}
catch 
{
    Console.WriteLine("Интернет умер");
}