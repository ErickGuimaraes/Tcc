using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MultiplayerNetworkSolution
{
    class Server
    {
        public static int maxPlayers { get; private set; }
        public static int port { get; private set; }
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        private static TcpListener tcpListener;

        public static void Start(int _maxPlayers, int _port)
        {
            maxPlayers = _maxPlayers;
            port = _port;

            Console.WriteLine("Server starting...");
            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectionCallback), null);

            Console.WriteLine($"Server running on {port}");
        }

        private static void TCPConnectionCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectionCallback), null);

            Console.WriteLine($"Incoming conection from {_client.Client.RemoteEndPoint}...");

            for (int i = 1; i <= maxPlayers; i++)
            {
                if(clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(_client);
                    return;
                }
            }

            //There is no more avaliable slots to new clients connections...
            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server is full");
        }

        private static void InitializeServerData()
        {
            for(int i = 1; i <= maxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }
        }
    }
}
