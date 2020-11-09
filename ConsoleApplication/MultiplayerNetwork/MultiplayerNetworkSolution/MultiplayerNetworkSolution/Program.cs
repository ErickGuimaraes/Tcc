using System;

namespace MultiplayerNetworkSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Multiplayer Network Server";

            Server.Start(50, 26950);
            Console.WriteLine("Ready");

            Console.ReadKey();
        }
    }
}
