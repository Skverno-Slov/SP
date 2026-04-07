using System.Net.Sockets;

var tspSocket =new Socket(AddressFamily.InterNetwork,
    SocketType.Stream,
    ProtocolType.Tcp);

var udpSocket = new Socket(AddressFamily.InterNetwork,
    SocketType.Dgram,
    ProtocolType.Udp);